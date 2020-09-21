using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSDKCS;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

//Added by Chris
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ThermalCam
{
    public partial class ThermalCam : Form
    {

        private IntPtr m_LoginID = IntPtr.Zero;
        private IntPtr m_RealPlayID = IntPtr.Zero;
        private IntPtr m_RealLoadID = IntPtr.Zero;

        private NET_DEVICEINFO_Ex m_DeviceInfo;
        private const int m_WaitTime = 5000;
        //private int nAnatomyTempEventNum = 0;
        private bool m_IsListenAlarm = false;

        private static fDisConnectCallBack m_DisConnectCallBack;//断线回调
        private static fHaveReConnectCallBack m_ReConnectCallBack;//重连回调
        private static fAnalyzerDataCallBack m_AnalyzerDataCallBack;//智能事件回调
        private static fMessCallBackEx m_AlarmCallBack;//警告事件回调

        //Added by Chris
        static readonly string DeviceConnectionString= ConfigurationManager.AppSettings["DEVICE_CONNECTION_STRING"];
        static readonly string TrustedCACertPath = ConfigurationManager.AppSettings["IOTEDGE_TRUSTED_CA_CERTIFICATE_PEM_PATH"];
        static int Sensor_Temperature_Min = Int32.Parse( ConfigurationManager.AppSettings["Sensor_Temperature_Min"]);//30;
        static int Sensor_Temperature_Max  = Int32.Parse( ConfigurationManager.AppSettings["Sensor_Temperature_Max"]);//45;
        static string SensorLocation = ConfigurationManager.AppSettings["Sensor_Location"];
        static string SensorIP = ConfigurationManager.AppSettings["Sensor_IP"];
        private static readonly string titleName = SensorLocation;

        const float Temperature_Threshhold = 37.2F;
        static string StorageConnString = ConfigurationManager.AppSettings["Storage_Connection_String"];
        static string EdgeStorageConnString = ConfigurationManager.AppSettings["Edge_Storage_Connection_String"];
        static string localContainerName = "cameraimages";
        public ThermalCam()
        {
            InitializeComponent();
        }

        private void ThermalCam_Load(object sender, EventArgs e)
        {
            Text = titleName;
            m_DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
            m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
            m_AnalyzerDataCallBack = new fAnalyzerDataCallBack(AnalyzerDataCallBack);
            m_AlarmCallBack = new fMessCallBackEx(AlarmCallBackEx);
                        ip_textBox.Text = SensorIP;
            //Added by Chris
            InstallCACert();

            try
            {
                ip_textBox.Text = SensorIP;
                //初始化
                NETClient.Init(m_DisConnectCallBack, IntPtr.Zero, null);
                //打开日志
                NET_LOG_SET_PRINT_INFO logInfo = new NET_LOG_SET_PRINT_INFO()
                {
                    dwSize = (uint)Marshal.SizeOf(typeof(NET_LOG_SET_PRINT_INFO))
                };
                NETClient.LogOpen(logInfo);
                //设置断线重连回调
                NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero);
                NETClient.SetDVRMessCallBack(m_AlarmCallBack, IntPtr.Zero);

             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
            }
        }

        #region Update UI 更新UI

        private void UpdateDisConnectUI()
        {
            this.Text = titleName + " -- Offline(离线)";
        }

        private void UpdateReConnectUI()
        {
            this.Text = titleName + " -- Online(在线)";
        }


        private void InitOrLogoutUI()
        {

            this.Text = titleName;
            btn_Login.Text = "Login(登录)";
            InitOrCloseOtherUI();
        }

        private void LoginUI()
        {
            this.Text = titleName + " -- Online(在线)";
            btn_Login.Text = "Logout(登出)";
            OpenOtherUI();
        }

        private void InitOrCloseOtherUI()
        {
            //其他控件操作初始化
            btn_Play.Enabled = false;
            //btn_RealLoad.Enabled = false;
            //btn_IsSupport.Enabled = false;
            //btn_GetConfig.Enabled = false;
            //btn_SetConfig.Enabled = false;
        }

        private void OpenOtherUI()
        {

            //其他控件操作使能
            btn_Play.Enabled = true;
            //btn_RealLoad.Enabled = true;
            //btn_IsSupport.Enabled = true;
            //btn_GetConfig.Enabled = true;
            //btn_SetConfig.Enabled = true;
        }

        #endregion

        #region CallBack 回调

        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateDisConnectUI);
        }

        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateReConnectUI);
        }

        private int AnalyzerDataCallBack(IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved)
        {
            switch (dwEventType)
            {
                // 人脸识别智能检测事件
                case (uint)EM_EVENT_IVS_TYPE.FACERECOGNITION:
                    {
                        NET_DEV_EVENT_FACERECOGNITION_INFO info = (NET_DEV_EVENT_FACERECOGNITION_INFO)Marshal.PtrToStructure(pEventInfo, typeof(NET_DEV_EVENT_FACERECOGNITION_INFO));
                        this.BeginInvoke(new Action(() =>
                        {
                            //显示事件信息
                            //nAnatomyTempEventNum++;
                            //var list_item = new ListViewItem();
                            //list_item.Text = nAnatomyTempEventNum.ToString();
                            //list_item.SubItems.Add(info.nChannelID.ToString());
                            //list_item.SubItems.Add(info.bEventAction.ToString());
                            //list_item.SubItems.Add(info.szName.ToString());
                            //list_item.SubItems.Add(info.nEventID.ToString());

                            //listView_EventInfo.BeginUpdate();
                            //listView_EventInfo.Items.Add(list_item);
                            //listView_EventInfo.EndUpdate();

                            // 区域内人员体温信息 
                            var list_item2 = new ListViewItem();
                            list_item2.Text = info.stuFaceData.bAnatomyTempDetect.ToString();
                            list_item2.SubItems.Add(info.stuFaceData.dbTemperature.ToString());
                            list_item2.SubItems.Add(info.stuFaceData.emTemperatureUnit.ToString());
                            list_item2.SubItems.Add(info.stuFaceData.bIsOverTemp.ToString());
                            list_item2.SubItems.Add(info.stuFaceData.bIsUnderTemp.ToString());
                            list_item2.SubItems.Add(info.stuFaceData.emMask.ToString());

                            listView_ManTempInfo.BeginUpdate();
                            listView_ManTempInfo.Items.Add(list_item2);
                            listView_ManTempInfo.EndUpdate();

                            // 可见光全景图起始位置：pBuffer + info.stuGlobalScenePicInfo.dwOffSet
                            // 可见光全景图长度：info.stuGlobalScenePicInfo.dwFileLenth
                            // 显示可见光全景图
                            if (info.bGlobalScenePic)
                            {
                                IntPtr buffer = IntPtr.Add(pBuffer, (int)info.stuGlobalScenePicInfo.dwOffSet);
                                ShowToPicCtrl(buffer, info.stuGlobalScenePicInfo.dwFileLenth, pb_VisSceneImage);
                            }

                            // 人脸小图信息：pBuffer + stThermalSceneImage.nOffset
                            // 人脸小图信息：stThermalSceneImage.nLength
                            // 显示小图信息
                            if (info.stuObject.stPicInfo.dwFileLenth > 0)
                            {
                                IntPtr buffer2 = IntPtr.Add(pBuffer, (int)info.stuObject.stPicInfo.dwOffSet);
                                ShowToPicCtrl(buffer2, info.stuObject.stPicInfo.dwFileLenth, pb_ThermalSceneImage);
                            }

                            // 识别到的人脸底库图起始位置：pBuffer + maxSimilarityPersonInfo.stPersonInfo.szFacePicInfo[0].dwOffSet
                            // 识别到的人脸底库图长度：maxSimilarityPersonInfo.stPersonInfo.szFacePicInfo[0].dwFileLenth
                            // 识别到的显示底库图图片
                            if (info.nCandidateNum > 0)
                            {
                                var candidatesInfo = info.stuCandidatesEx.ToList().OrderByDescending(p => p.bySimilarity).ToArray();
                                NET_CANDIDATE_INFOEX maxSimilarityPersonInfo = candidatesInfo[0];//最相似的候选人员信息
                                if (maxSimilarityPersonInfo.stPersonInfo.szFacePicInfo[0].dwFileLenth > 0)
                                {
                                    IntPtr buffer3 = IntPtr.Add(pBuffer, (int)maxSimilarityPersonInfo.stPersonInfo.szFacePicInfo[0].dwOffSet);

                                    /*******注意：由于界面空间有限，pictureBox3控件默认隐藏在界面最右方，调宽界面可以看到，客户可根据自己需要显示或调整*************/
                                    ShowToPicCtrl(buffer3, maxSimilarityPersonInfo.stPersonInfo.szFacePicInfo[0].dwFileLenth, pictureBoxIdentifiedFace);
                                }
                            }
                        }));

                    }
                    break;
                // 人体温智能检测事件
                case (uint)EM_EVENT_IVS_TYPE.ANATOMY_TEMP_DETECT:
                    {
                        NET_DEV_EVENT_ANATOMY_TEMP_DETECT_INFO info = (NET_DEV_EVENT_ANATOMY_TEMP_DETECT_INFO)Marshal.PtrToStructure(pEventInfo, typeof(NET_DEV_EVENT_ANATOMY_TEMP_DETECT_INFO));
                        this.BeginInvoke(new Action(() =>
                        {
                            //显示事件信息
                            //nAnatomyTempEventNum++;
                            //var list_item = new ListViewItem();
                            //list_item.Text = nAnatomyTempEventNum.ToString();
                            //list_item.SubItems.Add(info.nChannelID.ToString());
                            //list_item.SubItems.Add(info.nAction.ToString());
                            ////list_item.SubItems.Add(info.szName.ToString());
                            //list_item.SubItems.Add(info.nEventID.ToString());
                            //list_item.SubItems.Add(info.nSequence.ToString());
                            //list_item.SubItems.Add(info.nPresetID.ToString());
                            //list_item.SubItems.Add(info.PTS.ToString());

                            //listView_EventInfo.BeginUpdate();
                            //listView_EventInfo.Items.Add(list_item);
                            //listView_EventInfo.EndUpdate();

                            // 区域内人员体温信息 
                            var list_item2 = new ListViewItem();
                            list_item2.Text = info.stManTempInfo.nObjectID.ToString();

                            double dbHighTemp = info.stManTempInfo.dbHighTemp;
                            list_item2.SubItems.Add(dbHighTemp.ToString());
                            list_item2.SubItems.Add(info.stManTempInfo.nTempUnit.ToString());
                            //list_item2.SubItems.Add(info.stManTempInfo.bIsOverTemp.ToString());
                            //list_item2.SubItems.Add(info.stManTempInfo.bIsUnderTemp.ToString());

                            listView_ManTempInfo.BeginUpdate();
                            listView_ManTempInfo.Items.Add(list_item2);
                            listView_ManTempInfo.EndUpdate();

                            // 可见光全景图起始位置：pBuffer + stVisSceneImage.nOffset
                            // 可见光全景图长度：stVisSceneImage.nLength
                            // 显示可见光图
                            IntPtr bufferRGB = IntPtr.Add(pBuffer, (int)info.stVisSceneImage.nOffset);
                            ShowToPicCtrl(bufferRGB, info.stVisSceneImage.nLength, pb_VisSceneImage);

                            // 热成像全景图起始位置：pBuffer + stThermalSceneImage.nOffset
                            // 热成像全景图长度：stThermalSceneImage.nLength
                            // 显示热成像图片
                            IntPtr bufferThermal = IntPtr.Add(pBuffer, (int)info.stThermalSceneImage.nOffset);
                            ShowToPicCtrl(bufferThermal, info.stThermalSceneImage.nLength, pb_ThermalSceneImage);

                            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

                            BlobServiceClient blobServiceClient = new BlobServiceClient(EdgeStorageConnString);

                            string edgeDeviceId = blobServiceClient.Uri.Host;
                            if (deviceClient == null)
                            {
                                Console.WriteLine("Failed to create DeviceClient!");
                            }
                            else
                            {
                                string eventID = Guid.NewGuid().ToString();
                                
                                string rbg_FileName = String.Format("{0}-{1}.jpg", eventID, "RGB");
                                SendToBlobAsync(rbg_FileName, bufferRGB, info.stVisSceneImage.nLength, blobServiceClient);

                                string thermal_FileName = String.Format("{0}-{1}.jpg", eventID, "Thermal");
                                SendToBlobAsync(thermal_FileName, bufferThermal, info.stThermalSceneImage.nLength, blobServiceClient);
                                
                                SendEvents(deviceClient, eventID, dbHighTemp, rbg_FileName, thermal_FileName, edgeDeviceId).Wait();
                            }

                        }));
                    }
                    break;
                default:
                    break;
            }

            return 1;
        }

        private void ShowToPicCtrl(IntPtr buf, uint bufSize, PictureBox pb)
        {
            if (IntPtr.Zero != buf && bufSize > 0)
            {
                byte[] pic = new byte[bufSize];
                Marshal.Copy(buf, pic, 0, (int)bufSize);
                using (MemoryStream stream = new MemoryStream(pic))
                {
                    try
                    {
                        Image image = Image.FromStream(stream);
                        pb.Image = image;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private bool AlarmCallBackEx(int lCommand, IntPtr lLoginID, IntPtr pBuf, uint dwBufLen, IntPtr pchDVRIP, int nDVRPort, bool bAlarmAckFlag, int nEventID, IntPtr dwUser)
        {
            EM_ALARM_TYPE type = (EM_ALARM_TYPE)lCommand;
            switch (type)
            {
                case EM_ALARM_TYPE.ALARM_ANATOMY_TEMP_DETECT:
                    NET_ALARM_ANATOMY_TEMP_DETECT_INFO ana_info = (NET_ALARM_ANATOMY_TEMP_DETECT_INFO)Marshal.PtrToStructure(pBuf, typeof(NET_ALARM_ANATOMY_TEMP_DETECT_INFO));

                    this.BeginInvoke(new Action(() =>
                    {
                        //显示事件信息
                        var list_item = new ListViewItem();
                        list_item.Text = ana_info.nChannelID.ToString();
                        list_item.SubItems.Add(ana_info.nEventID.ToString());
                        list_item.SubItems.Add(ana_info.PTS.ToString());
                        list_item.SubItems.Add(ana_info.nPresetID.ToString());

                        //listView_AlarmInfo.BeginUpdate();
                        //listView_AlarmInfo.Items.Add(list_item);
                        //listView_AlarmInfo.EndUpdate();

                        // 区域内人员体温信息,与智能事件公用一个显示列表
                        var list_item2 = new ListViewItem();
                        list_item2.Text = ana_info.stManTempInfo.nObjectID.ToString();
                        list_item2.SubItems.Add(ana_info.stManTempInfo.dbHighTemp.ToString());
                        list_item2.SubItems.Add(ana_info.stManTempInfo.nTempUnit.ToString());
                        list_item2.SubItems.Add(ana_info.stManTempInfo.bIsOverTemp.ToString());
                        list_item2.SubItems.Add(ana_info.stManTempInfo.bIsUnderTemp.ToString());

                        //listView_ManTempInfo2.BeginUpdate();
                        //listView_ManTempInfo2.Items.Add(list_item2);
                        //listView_ManTempInfo2.EndUpdate();
                    }));
                    break;
                case EM_ALARM_TYPE.ALARM_REGULATOR_ABNORMAL:
                    NET_ALARM_REGULATOR_ABNORMAL_INFO reg_info = (NET_ALARM_REGULATOR_ABNORMAL_INFO)Marshal.PtrToStructure(pBuf, typeof(NET_ALARM_REGULATOR_ABNORMAL_INFO));

                    this.BeginInvoke(new Action(() =>
                    {
                        //显示事件信息
                        var list_item = new ListViewItem();
                        list_item.Text = reg_info.nChannelID.ToString();
                        list_item.SubItems.Add(reg_info.nEventID.ToString());
                        list_item.SubItems.Add(reg_info.PTS.ToString());
                        list_item.SubItems.Add(" ");
                        list_item.SubItems.Add(reg_info.nAction.ToString());
                        list_item.SubItems.Add(reg_info.szTypes.ToString());

                        //listView_AlarmInfo.BeginUpdate();
                        //listView_AlarmInfo.Items.Add(list_item);
                        //listView_AlarmInfo.EndUpdate();
                    }));
                    break;
            }

            return true;
        }

        #endregion

        private void btn_Login_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == m_LoginID)
            {
                ushort port = 0;
                try
                {
                    port = Convert.ToUInt16(port_textBox.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Input port error");
                    return;
                }
                m_DeviceInfo = new NET_DEVICEINFO_Ex();

                m_LoginID = NETClient.Login(ip_textBox.Text.Trim(), port, user_textBox.Text.Trim(), pwd_textBox.Text.Trim(), EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref m_DeviceInfo);
                if (IntPtr.Zero == m_LoginID)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                LoginUI();

                //Start listening 
                m_RealLoadID = NETClient.RealLoadPicture(m_LoginID, 0, (uint)EM_EVENT_IVS_TYPE.ALL, true, m_AnalyzerDataCallBack, IntPtr.Zero, IntPtr.Zero);

            }
            else
            {
                bool result = NETClient.Logout(m_LoginID);
                if (!result)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }

                m_LoginID = IntPtr.Zero;
                InitOrLogoutUI();
            }
        }


        private void btn_Play_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == m_RealPlayID)
            {
                // realplay 监视
                //针对热成像设备，通道号0为普通画面，通道号1为热成像画面
                int videoChannel = 0;
                if (chkBoxThermalChannel.Checked)
                    videoChannel = 1;
                else
                    videoChannel = 0;
                m_RealPlayID = NETClient.RealPlay(m_LoginID, videoChannel, pb_PlayBox.Handle, EM_RealPlayType.Realplay);
                if (IntPtr.Zero == m_RealPlayID)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                btn_Play.Text = "Stop(停止)";
            }
            else
            {
                // stop realplay 关闭监视
                bool ret = NETClient.StopRealPlay(m_RealPlayID);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_RealPlayID = IntPtr.Zero;
                btn_Play.Text = "Play(播放)";
            }
        }

        private void btn_IsSupport_Click(object sender, EventArgs e)
        {
            //判断摄像头是否支持黑体异常报警
            NET_IN_GET_HUMAN_RADIO_CAPS stuIn = new NET_IN_GET_HUMAN_RADIO_CAPS();
            stuIn.dwSize = (uint)Marshal.SizeOf(stuIn);
            //针对热成像设备，若是单通道，通道号为0，如果是双通道，通道号为1
            if (m_DeviceInfo.nChanNum == 1)
            {
                stuIn.nChannel = 0;
            }else if (m_DeviceInfo.nChanNum == 2)
            {
                stuIn.nChannel = 1;
            }

            NET_OUT_GET_HUMAN_RADIO_CAPS stuOut = new NET_OUT_GET_HUMAN_RADIO_CAPS();
            stuOut.dwSize = (uint)Marshal.SizeOf(stuOut);

            //bool bRet = NETClient.GetHumanRadioCaps(m_LoginID, ref stuIn, ref stuOut, 5000);
            //if (bRet)
            //{
            //    cb_Support.Checked = stuOut.bSupportRegulatorAlarm;
            //}
            //else
            //{
            //    MessageBox.Show(NETClient.GetLastError());
            //}
        }

        private void btn_StartListen_Click(object sender, EventArgs e)
        {
            if (m_IsListenAlarm)
            {
                m_IsListenAlarm = false;
                bool ret = NETClient.StopListen(m_LoginID);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                //btn_StartListen.Text = "ListenAlarm(监听警告)";
            }
            else
            {
                m_IsListenAlarm = true;
                bool ret = NETClient.StartListen(m_LoginID);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                //btn_StartListen.Text = "Stop(停止监听)";
            }
        }

        private NET_CFG_RADIO_REGULATOR getinfo = new NET_CFG_RADIO_REGULATOR();

        private void btn_GetConfig_Click(object sender, EventArgs e)
        {
            NET_CFG_RADIO_REGULATOR info = new NET_CFG_RADIO_REGULATOR();
            info.dwSize = (uint)Marshal.SizeOf(info);
            object obj = info;
            bool ret = NETClient.GetOperateConfig(m_LoginID, EM_CFG_OPERATE_TYPE.RADIO_REGULATOR, 1, ref obj, typeof(NET_CFG_RADIO_REGULATOR), 5000);
            if (!ret)
            {
                MessageBox.Show(NETClient.GetLastError());
                return;
            }
            getinfo = (NET_CFG_RADIO_REGULATOR)obj;
            MessageBox.Show("获取成功！");
        }

        private void btn_SetConfig_Click(object sender, EventArgs e)
        {
            getinfo.bEnable = true;

            object obj = getinfo;
            bool ret = NETClient.SetOperateConfig(m_LoginID, EM_CFG_OPERATE_TYPE.RADIO_REGULATOR, 1, obj, typeof(NET_CFG_RADIO_REGULATOR), 5000);
            if (!ret)
            {
                MessageBox.Show(NETClient.GetLastError());
                return;
            }
            MessageBox.Show("设置成功！");
        }

        /// <summary>
        /// Added by Chris
        /// Add certificate in local cert store for use by downstream device
        /// client for secure connection to IoT Edge runtime.
        ///
        ///    Note: On Windows machines, if you have not run this from an Administrator prompt,
        ///    a prompt will likely come up to confirm the installation of the certificate.
        ///    This usually happens the first time a certificate will be installed.
        /// </summary>
        static void InstallCACert()
        {
            
            if (!string.IsNullOrWhiteSpace(TrustedCACertPath))
            {
                Console.WriteLine("User configured CA certificate path: {0}", TrustedCACertPath);
                if (!File.Exists(TrustedCACertPath))
                {
                    // cannot proceed further without a proper cert file
                    Console.WriteLine("Certificate file not found: {0}", TrustedCACertPath);
                    throw new InvalidOperationException("Invalid certificate file.");
                }
                else
                {
                    Console.WriteLine("Attempting to install CA certificate: {0}", TrustedCACertPath);
                    X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadWrite);
                    store.Add(new X509Certificate2(X509Certificate.CreateFromCertFile(TrustedCACertPath)));
                    Console.WriteLine("Successfully added certificate: {0}", TrustedCACertPath);
                    store.Close();
                }
            }
            else
            {
                Console.WriteLine("CA_CERTIFICATE_PATH was not set or null, not installing any CA certificate");
            }
        }

        /// <summary>
        /// Added by Chris
        /// Send telemetry data, (random temperature and humidity data samples).
        /// to the IoT Edge runtime. The number of messages to be sent is determined
        /// by environment variable MESSAGE_COUNT.
        /// </summary>
        static async Task SendEvents(DeviceClient deviceClient, string eventID, double temperatureSensorReading,string rgb_FileName, string thermal_FileName, string edgeDeviceId)
        {
            //Random rnd = new Random();

            var messageData = new MessageBody
            {
                SensorTime = DateTime.Now.ToLocalTime(),

                SensorLocation = SensorLocation,

                Temperature_Threshhold = Temperature_Threshhold,

                Sensor_Temperature_Min = Sensor_Temperature_Min,

                Sensor_Temperature_Max = Sensor_Temperature_Max,

                Sensor_Temperature_Reading = (float)temperatureSensorReading,

                Subject_Temperature_Verification = 0,//(float)Math.Round(Temperature_Threshhold - rnd.NextDouble(), 1),

                IR_Picture_Id = thermal_FileName,
                RGB_Picture_Id = rgb_FileName,

                Subject_Id = "",
                Gatekeeper_Id = "",

                Edge_Device_Id = edgeDeviceId

            };

            string dataBuffer = JsonConvert.SerializeObject(messageData);
            Microsoft.Azure.Devices.Client.Message eventMessage = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(dataBuffer));
            eventMessage.Properties.Add("temperatureAlert", (temperatureSensorReading > Temperature_Threshhold) ? "true" : "false");
            eventMessage.Properties.Add("correlationID", eventID);
            
            try
            {
                await deviceClient.SendEventAsync(eventMessage).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

        //ShowToPicCtrl(IntPtr buf, uint bufSize, PictureBox pb)
        static async void SendToBlobAsync(DeviceClient deviceClient, string imageType, IntPtr buf, uint bufSize)
        {
            string eventID = Guid.NewGuid().ToString();
            string fileName = String.Format("{0}-{1}.jpg",eventID,imageType);

            if (IntPtr.Zero != buf && bufSize > 0)
            {
                byte[] pic = new byte[bufSize];
                Marshal.Copy(buf, pic, 0, (int)bufSize);
                using (MemoryStream stream = new MemoryStream(pic))
                {
                    try
                    {
                        await deviceClient.UploadToBlobAsync(fileName, stream);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        static async void SendToBlobAsync(string fileName, IntPtr buf, uint bufSize, BlobServiceClient blobServiceClient)
        {
            
            if (IntPtr.Zero != buf && bufSize > 0)
            {
                byte[] pic = new byte[bufSize];
                Marshal.Copy(buf, pic, 0, (int)bufSize);
                //unique name for the container


                using (MemoryStream stream = new MemoryStream(pic))
                {
                    try
                    {
                        // Create a BlobServiceClient object which will be used to create a container client
                        //BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnString);
                        //BlobServiceClient blobServiceClient = new BlobServiceClient(EdgeStorageConnString);                                      

                        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(localContainerName);
                        if( !containerClient.Exists())
                           await blobServiceClient.CreateBlobContainerAsync(localContainerName);

                        BlobClient blobClient = containerClient.GetBlobClient(fileName);                     
                        await blobClient.UploadAsync(stream, true);
                        
                        stream.Close();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

    }
}
