// Copyright (c) Microsoft. All rights reserved.
namespace EdgeModule_Simulated_IR_Cam
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using DotNetty.Common.Utilities;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    using Microsoft.Azure.Devices.Edge.Util;
    using Microsoft.Azure.Devices.Edge.Util.Concurrency;
    using Microsoft.Azure.Devices.Edge.Util.TransientFaultHandling;
    using Microsoft.Azure.Devices.Shared;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using ExponentialBackoff = Microsoft.Azure.Devices.Edge.Util.TransientFaultHandling.ExponentialBackoff;

    class Program
    {
        static int m_Sensor_Temperature_Min { get; set; } = 30;
        static int m_Sensor_Temperature_Max { get; set; } = 45;
        static int m_MessageInterval { get; set; } = 8; // TimeSpan.FromSeconds(5);
        static bool m_ActivateCam { get; set; } = true;
        static string m_SensorLocation { get; set; } = "Sim-MTC-Beijing";

        const float Temperature_Threshhold = 37.2F;
        //const string MessageCountConfigKey = "MessageCount";
        //const string SendDataConfigKey = "SendData";
        //const string SendIntervalConfigKey = "SendInterval";

        static readonly ITransientErrorDetectionStrategy DefaultTimeoutErrorDetectionStrategy =
            new DelegateErrorDetectionStrategy(ex => ex.HasTimeoutException());

        static readonly RetryStrategy DefaultTransientRetryStrategy =
            new ExponentialBackoff(
                5,
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(60),
                TimeSpan.FromSeconds(4));

        static readonly Guid BatchId = Guid.NewGuid();
        static readonly AtomicBoolean Reset = new AtomicBoolean(false);
        static readonly Random rnd = new Random();


        //public enum ControlCommandEnum
        //{
        //    Reset = 0,
        //    NoOperation = 1
        //}

        public static int Main() => MainAsync().Result;

        static async Task<int> MainAsync()
        {
            Console.WriteLine("IR Cam Simulator Main() started.");

            Console.WriteLine(
                $"Initializing simulated temperature sensor to send "
                + $"messages, at an interval of {m_MessageInterval} seconds.\n"
                + $"To change this, set the device twin variable 'MessageInterval' to the number of messages that should be sent (set it to -1 to send unlimited messages).");

            TransportType transportType = TransportType.Amqp_Tcp_Only;

            ModuleClient moduleClient = await CreateModuleClientAsync(
                transportType,
                DefaultTimeoutErrorDetectionStrategy,
                DefaultTransientRetryStrategy);
            await moduleClient.OpenAsync();
            await moduleClient.SetMethodHandlerAsync("reset", ResetMethod, null);

            (CancellationTokenSource cts, ManualResetEventSlim completed, Option<object> handler) = ShutdownHandler.Init(TimeSpan.FromSeconds(m_MessageInterval), null);

            Twin currentTwinProperties = await moduleClient.GetTwinAsync();                     
            await OnDesiredPropertiesUpdate(currentTwinProperties.Properties.Desired, moduleClient);

            ModuleClient userContext = moduleClient;
            await moduleClient.SetDesiredPropertyUpdateCallbackAsync(OnDesiredPropertiesUpdate, userContext);
            await SendEvents(moduleClient, cts);
            await cts.Token.WhenCanceled();

            completed.Set();
            handler.ForEach(h => GC.KeepAlive(h));
            Console.WriteLine("IR Cam Simulator Main() finished.");
            return 0;
        }      
        static Task<MethodResponse> ResetMethod(MethodRequest methodRequest, object userContext)
        {
            Console.WriteLine("Received direct method call to reset temperature sensor...");
            Reset.Set(true);
            var response = new MethodResponse((int)HttpStatusCode.OK);
            return Task.FromResult(response);
        }
        /// <summary>
        /// Module behavior:
        ///        Sends data periodically (with default frequency of 5 seconds).
        ///        Data trend:
        ///         - Machine Temperature regularly rises from 21C to 100C in regularly with jitter
        ///         - Machine Pressure correlates with Temperature 1 to 10psi
        ///         - Ambient temperature stable around 21C
        ///         - Humidity is stable with tiny jitter around 25%
        ///                Method for resetting the data stream.
        /// </summary>
        static async Task SendEvents(
            ModuleClient moduleClient,
            //SimulatorParameters sim,
            CancellationTokenSource cts)
        {
            int count = 1;
            double simTemp = m_Sensor_Temperature_Min;

            while (!cts.Token.IsCancellationRequested)
            {
                if (Reset)
                {
                    simTemp = m_Sensor_Temperature_Min;
                    count = 1;
                    Reset.Set(false);
                }

                //currentTemp = rnd.Next(Sensor_Temperature_Min, Sensor_Temperature_Max - 1) + (float)rnd.NextDouble();

                if (m_ActivateCam)
                {
                    simTemp = rnd.Next(m_Sensor_Temperature_Min, m_Sensor_Temperature_Max - 1) + rnd.NextDouble();

                    var messageData = new MessageBody
                    {
                        SensorTime = DateTime.Now,

                        SensorLocation = m_SensorLocation,

                        Temperature_Threshhold = Temperature_Threshhold,

                        Sensor_Temperature_Min = m_Sensor_Temperature_Min,

                        Sensor_Temperature_Max = m_Sensor_Temperature_Max,

                        Sensor_Temperature_Reading = (float)Math.Round(simTemp, 1),

                        Subject_Temperature_Verification = (float)Math.Round(Temperature_Threshhold - rnd.NextDouble(), 1),

                        IR_Picture_Id = "IR_Picture1.jpg",
                        RGB_Picture_Id = " RGB_Picture1.jpg",

                        Subject_Id = "person1",
                        Gatekeeper_Id = "Security Guard 007"

                    };

                    string dataBuffer = JsonConvert.SerializeObject(messageData);
                    var eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
                    eventMessage.Properties.Add("sequenceNumber", count.ToString());
                    eventMessage.Properties.Add("batchId", BatchId.ToString());
                    Console.WriteLine($"\t{DateTime.Now.ToLocalTime()}> Sending message: {count}, Body: [{dataBuffer}]");

                    /*******************************************************
                    * ****************            **************************
                    ***************** SENDING DATA *************************
                    * ****************            **************************
                    * ******************************************************/
                    await moduleClient.SendEventAsync("output", eventMessage);
                    Console.WriteLine($"\t{DateTime.Now.ToLocalTime()}> Message {count}: Sent Seuccessfully!");
                    count++;
                }

                TimeSpan pauseTime = TimeSpan.FromSeconds(m_MessageInterval);
                await Task.Delay(pauseTime, cts.Token);
            }
        }
        static Task OnDesiredPropertiesUpdate(TwinCollection desiredPropertiesPatch, object userContext)
        {
            try
            {
                // At this point just update the configure configuration.
                if (desiredPropertiesPatch.Contains("MessageInterval"))
                {
                    m_MessageInterval = (int)desiredPropertiesPatch["MessageInterval"];
                }
                if (desiredPropertiesPatch.Contains("SensorLocation"))
                {
                    m_SensorLocation = (string)desiredPropertiesPatch["SensorLocation"];
                    Console.WriteLine("Loading Desired SensorLocation: {0}", m_SensorLocation);
                }
                if (desiredPropertiesPatch.Contains("ActivateCam"))
                {
                    bool desiredSendDataValue = (bool)desiredPropertiesPatch["ActivateCam"];
                    if (desiredSendDataValue != m_ActivateCam && !desiredSendDataValue)
                    {
                        Console.WriteLine("Sending data disabled. Change twin configuration to start sending again.");
                    }

                    m_ActivateCam = desiredSendDataValue;
                }
                if (desiredPropertiesPatch.Contains("Sensor_Temperature_Min"))
                {
                    m_Sensor_Temperature_Min = (int)desiredPropertiesPatch["Sensor_Temperature_Min"];
                }
                if (desiredPropertiesPatch.Contains("Sensor_Temperature_Max"))
                {
                    m_Sensor_Temperature_Max = (int)desiredPropertiesPatch["Sensor_Temperature_Max"];
                }
                var moduleClient = (ModuleClient)userContext;
                var patch = new TwinCollection(
                    $"{{ \"ActivateCam\":{m_ActivateCam.ToString().ToLower()}, \"MessageInterval\": {m_MessageInterval}, \"Sensor_Temperature_Min\": {m_Sensor_Temperature_Min}, \"Sensor_Temperature_Max\": {m_Sensor_Temperature_Max}, \"SensorLocation\": \"{m_SensorLocation}\"}}"
                    );

                //patch["SensorLocation"] = m_SensorLocation;
                Console.WriteLine("Updating Desired Twin Properties: {0}", patch.ToJson());
                moduleClient.UpdateReportedPropertiesAsync(patch); // Just report back last desired property.
            }
            catch (AggregateException ex)
            {
                foreach (Exception exception in ex.InnerExceptions)
                {
                    Console.WriteLine();
                    Console.WriteLine("Error when receiving desired property: {0}", exception);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error when receiving desired property: {0}", ex.Message);
            }

            return Task.CompletedTask;
        }
        static async Task<ModuleClient> CreateModuleClientAsync(
            TransportType transportType,
            ITransientErrorDetectionStrategy transientErrorDetectionStrategy = null,
            RetryStrategy retryStrategy = null)
        {
            var retryPolicy = new RetryPolicy(transientErrorDetectionStrategy, retryStrategy);
            retryPolicy.Retrying += (_, args) => { Console.WriteLine($"[Error] Retry {args.CurrentRetryCount} times to create module client and failed with exception:{Environment.NewLine}{args.LastException}"); };
            ModuleClient client = await retryPolicy.ExecuteAsync(
                async () =>
                {
                    ITransportSettings[] GetTransportSettings()
                    {
                        switch (transportType)
                        {
                            case TransportType.Mqtt:
                            case TransportType.Mqtt_Tcp_Only:
                                return new ITransportSettings[] { new MqttTransportSettings(TransportType.Mqtt_Tcp_Only) };
                            case TransportType.Mqtt_WebSocket_Only:
                                return new ITransportSettings[] { new MqttTransportSettings(TransportType.Mqtt_WebSocket_Only) };
                            case TransportType.Amqp_WebSocket_Only:
                                return new ITransportSettings[] { new AmqpTransportSettings(TransportType.Amqp_WebSocket_Only) };
                            default:
                                return new ITransportSettings[] { new AmqpTransportSettings(TransportType.Amqp_Tcp_Only) };
                        }
                    }
                    ITransportSettings[] settings = GetTransportSettings();
                    Console.WriteLine($"[Information]: Trying to initialize module client using transport type [{transportType}].");
                    ModuleClient moduleClient = await ModuleClient.CreateFromEnvironmentAsync(settings);
                    await moduleClient.OpenAsync();
                    Console.WriteLine($"[Information]: Successfully initialized module client of transport type [{transportType}].");
                    return moduleClient;
                });
            return client;
        }       
    }
}
