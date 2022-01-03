// Decompiled with JetBrains decompiler
// Type: PS3SaveEditor.USB
// Assembly: SWPS4MAX, Version=1.0.7646.26709, Culture=neutral, PublicKeyToken=null
// MVID: 185BF9ED-B762-4AE8-B9E6-BAC5BF775B8B
// Assembly location: C:\Program Files (x86)\DataPower\Save Wizard for PS4 MAX\SWPS4MAX.exe

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text;

namespace PS3SaveEditor
{
  public class USB
  {
    private const int IOCTL_STORAGE_GET_DEVICE_NUMBER = 2953344;
    private const string GUID_DEVINTERFACE_DISK = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
    private const int GENERIC_WRITE = 1073741824;
    private const int FILE_SHARE_READ = 1;
    private const int FILE_SHARE_WRITE = 2;
    private const int OPEN_EXISTING = 3;
    private const int INVALID_HANDLE_VALUE = -1;
    private const int IOCTL_GET_HCD_DRIVERKEY_NAME = 2229284;
    private const int IOCTL_USB_GET_ROOT_HUB_NAME = 2229256;
    private const int IOCTL_USB_GET_NODE_INFORMATION = 2229256;
    private const int IOCTL_USB_GET_NODE_CONNECTION_INFORMATION_EX = 2229320;
    private const int IOCTL_USB_GET_DESCRIPTOR_FROM_NODE_CONNECTION = 2229264;
    private const int IOCTL_USB_GET_NODE_CONNECTION_NAME = 2229268;
    private const int IOCTL_USB_GET_NODE_CONNECTION_DRIVERKEY_NAME = 2229280;
    private const int USB_DEVICE_DESCRIPTOR_TYPE = 1;
    private const int USB_STRING_DESCRIPTOR_TYPE = 3;
    private const int BUFFER_SIZE = 2048;
    private const int MAXIMUM_USB_STRING_LENGTH = 255;
    private const string GUID_DEVINTERFACE_HUBCONTROLLER = "3abf6f2d-71c4-462a-8a92-1e6861e6af27";
    private const string REGSTR_KEY_USB = "USB";
    private const int DIGCF_PRESENT = 2;
    private const int DIGCF_ALLCLASSES = 4;
    private const int DIGCF_DEVICEINTERFACE = 16;
    private const int SPDRP_DRIVER = 9;
    private const int SPDRP_DEVICEDESC = 0;
    private const int REG_SZ = 1;

    public static List<USB.USBDevice> GetConnectedDevices()
    {
      List<USB.USBDevice> DevList = new List<USB.USBDevice>();
      foreach (USB.USBController hostController in USB.GetHostControllers())
        USB.ListHub(hostController.GetRootHub(), DevList);
      return DevList;
    }

    private static void ListHub(USB.USBHub Hub, List<USB.USBDevice> DevList)
    {
      foreach (USB.USBPort port in Hub.GetPorts())
      {
        if (port.IsHub)
          USB.ListHub(port.GetHub(), DevList);
        else if (port.IsDeviceConnected)
          DevList.Add(port.GetDevice());
      }
    }

    public static USB.USBDevice FindDeviceByDriverKeyName(string DriverKeyName)
    {
      USB.USBDevice FoundDevice = (USB.USBDevice) null;
      foreach (USB.USBController hostController in USB.GetHostControllers())
      {
        USB.SearchHubDriverKeyName(hostController.GetRootHub(), ref FoundDevice, DriverKeyName);
        if (FoundDevice != null)
          break;
      }
      return FoundDevice;
    }

    private static void SearchHubDriverKeyName(
      USB.USBHub Hub,
      ref USB.USBDevice FoundDevice,
      string DriverKeyName)
    {
      foreach (USB.USBPort port in Hub.GetPorts())
      {
        if (port.IsHub)
          USB.SearchHubDriverKeyName(port.GetHub(), ref FoundDevice, DriverKeyName);
        else if (port.IsDeviceConnected)
        {
          USB.USBDevice device = port.GetDevice();
          if (device.DeviceDriverKey == DriverKeyName)
          {
            FoundDevice = device;
            break;
          }
        }
      }
    }

    public static USB.USBDevice FindDeviceByInstanceID(string InstanceID)
    {
      USB.USBDevice FoundDevice = (USB.USBDevice) null;
      foreach (USB.USBController hostController in USB.GetHostControllers())
      {
        USB.SearchHubInstanceID(hostController.GetRootHub(), ref FoundDevice, InstanceID);
        if (FoundDevice != null)
          break;
      }
      return FoundDevice;
    }

    private static void SearchHubInstanceID(
      USB.USBHub Hub,
      ref USB.USBDevice FoundDevice,
      string InstanceID)
    {
      foreach (USB.USBPort port in Hub.GetPorts())
      {
        if (port.IsHub)
          USB.SearchHubInstanceID(port.GetHub(), ref FoundDevice, InstanceID);
        else if (port.IsDeviceConnected)
        {
          USB.USBDevice device = port.GetDevice();
          if (device.InstanceID == InstanceID)
          {
            FoundDevice = device;
            break;
          }
        }
      }
    }

    [DllImport("setupapi.dll")]
    private static extern int CM_Get_Parent(out IntPtr pdnDevInst, int dnDevInst, int ulFlags);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
    private static extern int CM_Get_Device_ID(
      IntPtr dnDevInst,
      IntPtr Buffer,
      int BufferLen,
      int ulFlags);

    public static USB.USBDevice FindDriveLetter(string DriveLetter)
    {
      USB.USBDevice usbDevice = (USB.USBDevice) null;
      string InstanceID = "";
      int deviceNumber = USB.GetDeviceNumber("\\\\.\\" + DriveLetter.TrimEnd('\\'));
      if (deviceNumber < 0)
        return usbDevice;
      Guid guid = new Guid("53f56307-b6bf-11d0-94f2-00a0c91efb8b");
      IntPtr classDevs = USB.SetupDiGetClassDevs(ref guid, 0, IntPtr.Zero, 18);
      if (classDevs.ToInt32() != -1)
      {
        int MemberIndex = 0;
        bool flag;
        do
        {
          USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new USB.SP_DEVICE_INTERFACE_DATA();
          DeviceInterfaceData.cbSize = Marshal.SizeOf((object) DeviceInterfaceData);
          flag = USB.SetupDiEnumDeviceInterfaces(classDevs, IntPtr.Zero, ref guid, MemberIndex, ref DeviceInterfaceData);
          if (flag)
          {
            USB.SP_DEVINFO_DATA DeviceInfoData = new USB.SP_DEVINFO_DATA();
            DeviceInfoData.cbSize = Marshal.SizeOf((object) DeviceInfoData);
            USB.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData = new USB.SP_DEVICE_INTERFACE_DETAIL_DATA();
            DeviceInterfaceDetailData.cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;
            int RequiredSize = 0;
            int num1 = 2048;
            if (USB.SetupDiGetDeviceInterfaceDetail(classDevs, ref DeviceInterfaceData, ref DeviceInterfaceDetailData, num1, ref RequiredSize, ref DeviceInfoData) && USB.GetDeviceNumber(DeviceInterfaceDetailData.DevicePath) == deviceNumber)
            {
              IntPtr pdnDevInst;
              USB.CM_Get_Parent(out pdnDevInst, DeviceInfoData.DevInst, 0);
              IntPtr num2 = Marshal.AllocHGlobal(num1);
              USB.CM_Get_Device_ID(pdnDevInst, num2, num1, 0);
              InstanceID = Marshal.PtrToStringAuto(num2);
              Marshal.FreeHGlobal(num2);
              break;
            }
          }
          ++MemberIndex;
        }
        while (flag);
        USB.SetupDiDestroyDeviceInfoList(classDevs);
      }
      if (InstanceID.StartsWith("USB\\"))
        usbDevice = USB.FindDeviceByInstanceID(InstanceID);
      return usbDevice;
    }

    private static int GetDeviceNumber(string DevicePath)
    {
      int num1 = -1;
      IntPtr file = USB.CreateFile(DevicePath.TrimEnd('\\'), 0, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
      if (file.ToInt32() != -1)
      {
        int num2 = Marshal.SizeOf((object) new USB.STORAGE_DEVICE_NUMBER());
        IntPtr num3 = Marshal.AllocHGlobal(num2);
        if (USB.DeviceIoControl(file, 2953344, IntPtr.Zero, 0, num3, num2, out int _, IntPtr.Zero))
        {
          USB.STORAGE_DEVICE_NUMBER structure = (USB.STORAGE_DEVICE_NUMBER) Marshal.PtrToStructure(num3, typeof (USB.STORAGE_DEVICE_NUMBER));
          num1 = (structure.DeviceType << 8) + structure.DeviceNumber;
        }
        Marshal.FreeHGlobal(num3);
        USB.CloseHandle(file);
      }
      return num1;
    }

    [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SetupDiGetClassDevs(
      ref Guid ClassGuid,
      int Enumerator,
      IntPtr hwndParent,
      int Flags);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SetupDiGetClassDevs(
      int ClassGuid,
      string Enumerator,
      IntPtr hwndParent,
      int Flags);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiEnumDeviceInterfaces(
      IntPtr DeviceInfoSet,
      IntPtr DeviceInfoData,
      ref Guid InterfaceClassGuid,
      int MemberIndex,
      ref USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiGetDeviceInterfaceDetail(
      IntPtr DeviceInfoSet,
      ref USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
      ref USB.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData,
      int DeviceInterfaceDetailDataSize,
      ref int RequiredSize,
      ref USB.SP_DEVINFO_DATA DeviceInfoData);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiGetDeviceRegistryProperty(
      IntPtr DeviceInfoSet,
      ref USB.SP_DEVINFO_DATA DeviceInfoData,
      int iProperty,
      ref int PropertyRegDataType,
      IntPtr PropertyBuffer,
      int PropertyBufferSize,
      ref int RequiredSize);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiEnumDeviceInfo(
      IntPtr DeviceInfoSet,
      int MemberIndex,
      ref USB.SP_DEVINFO_DATA DeviceInfoData);

    [DllImport("setupapi.dll", SetLastError = true)]
    private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

    [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool SetupDiGetDeviceInstanceId(
      IntPtr DeviceInfoSet,
      ref USB.SP_DEVINFO_DATA DeviceInfoData,
      StringBuilder DeviceInstanceId,
      int DeviceInstanceIdSize,
      out int RequiredSize);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool DeviceIoControl(
      IntPtr hDevice,
      int dwIoControlCode,
      IntPtr lpInBuffer,
      int nInBufferSize,
      IntPtr lpOutBuffer,
      int nOutBufferSize,
      out int lpBytesReturned,
      IntPtr lpOverlapped);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CreateFile(
      string lpFileName,
      int dwDesiredAccess,
      int dwShareMode,
      IntPtr lpSecurityAttributes,
      int dwCreationDisposition,
      int dwFlagsAndAttributes,
      IntPtr hTemplateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    public static ReadOnlyCollection<USB.USBController> GetHostControllers()
    {
      List<USB.USBController> usbControllerList = new List<USB.USBController>();
      Guid guid = new Guid("3abf6f2d-71c4-462a-8a92-1e6861e6af27");
      IntPtr classDevs = USB.SetupDiGetClassDevs(ref guid, 0, IntPtr.Zero, 18);
      if (classDevs.ToInt32() != -1)
      {
        IntPtr num = Marshal.AllocHGlobal(2048);
        int MemberIndex = 0;
        bool flag;
        do
        {
          USB.USBController usbController = new USB.USBController();
          usbController.ControllerIndex = MemberIndex;
          USB.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new USB.SP_DEVICE_INTERFACE_DATA();
          DeviceInterfaceData.cbSize = Marshal.SizeOf((object) DeviceInterfaceData);
          flag = USB.SetupDiEnumDeviceInterfaces(classDevs, IntPtr.Zero, ref guid, MemberIndex, ref DeviceInterfaceData);
          if (flag)
          {
            USB.SP_DEVINFO_DATA DeviceInfoData = new USB.SP_DEVINFO_DATA();
            DeviceInfoData.cbSize = Marshal.SizeOf((object) DeviceInfoData);
            USB.SP_DEVICE_INTERFACE_DETAIL_DATA DeviceInterfaceDetailData = new USB.SP_DEVICE_INTERFACE_DETAIL_DATA();
            DeviceInterfaceDetailData.cbSize = IntPtr.Size == 4 ? 4 + Marshal.SystemDefaultCharSize : 8;
            int RequiredSize1 = 0;
            int DeviceInterfaceDetailDataSize = 2048;
            if (USB.SetupDiGetDeviceInterfaceDetail(classDevs, ref DeviceInterfaceData, ref DeviceInterfaceDetailData, DeviceInterfaceDetailDataSize, ref RequiredSize1, ref DeviceInfoData))
            {
              usbController.ControllerDevicePath = DeviceInterfaceDetailData.DevicePath;
              int RequiredSize2 = 0;
              int PropertyRegDataType = 1;
              if (USB.SetupDiGetDeviceRegistryProperty(classDevs, ref DeviceInfoData, 0, ref PropertyRegDataType, num, 2048, ref RequiredSize2))
                usbController.ControllerDeviceDesc = Marshal.PtrToStringAuto(num);
              if (USB.SetupDiGetDeviceRegistryProperty(classDevs, ref DeviceInfoData, 9, ref PropertyRegDataType, num, 2048, ref RequiredSize2))
                usbController.ControllerDriverKeyName = Marshal.PtrToStringAuto(num);
            }
            usbControllerList.Add(usbController);
          }
          ++MemberIndex;
        }
        while (flag);
        Marshal.FreeHGlobal(num);
        USB.SetupDiDestroyDeviceInfoList(classDevs);
      }
      return new ReadOnlyCollection<USB.USBController>((IList<USB.USBController>) usbControllerList);
    }

    private static string GetDescriptionByKeyName(string DriverKeyName)
    {
      string str1 = "";
      IntPtr classDevs = USB.SetupDiGetClassDevs(0, nameof (USB), IntPtr.Zero, 6);
      if (classDevs.ToInt32() != -1)
      {
        IntPtr num = Marshal.AllocHGlobal(2048);
        int MemberIndex = 0;
        bool flag;
        do
        {
          USB.SP_DEVINFO_DATA DeviceInfoData = new USB.SP_DEVINFO_DATA();
          DeviceInfoData.cbSize = Marshal.SizeOf((object) DeviceInfoData);
          flag = USB.SetupDiEnumDeviceInfo(classDevs, MemberIndex, ref DeviceInfoData);
          if (flag)
          {
            int RequiredSize = 0;
            int PropertyRegDataType = 1;
            string str2 = "";
            if (USB.SetupDiGetDeviceRegistryProperty(classDevs, ref DeviceInfoData, 9, ref PropertyRegDataType, num, 2048, ref RequiredSize))
              str2 = Marshal.PtrToStringAuto(num);
            if (str2 == DriverKeyName)
            {
              if (USB.SetupDiGetDeviceRegistryProperty(classDevs, ref DeviceInfoData, 0, ref PropertyRegDataType, num, 2048, ref RequiredSize))
              {
                str1 = Marshal.PtrToStringAuto(num);
                break;
              }
              break;
            }
          }
          ++MemberIndex;
        }
        while (flag);
        Marshal.FreeHGlobal(num);
        USB.SetupDiDestroyDeviceInfoList(classDevs);
      }
      return str1;
    }

    private static string GetInstanceIDByKeyName(string DriverKeyName)
    {
      string str1 = "";
      IntPtr classDevs = USB.SetupDiGetClassDevs(0, nameof (USB), IntPtr.Zero, 6);
      if (classDevs.ToInt32() != -1)
      {
        IntPtr num1 = Marshal.AllocHGlobal(2048);
        int MemberIndex = 0;
        bool flag;
        do
        {
          USB.SP_DEVINFO_DATA DeviceInfoData = new USB.SP_DEVINFO_DATA();
          DeviceInfoData.cbSize = Marshal.SizeOf((object) DeviceInfoData);
          flag = USB.SetupDiEnumDeviceInfo(classDevs, MemberIndex, ref DeviceInfoData);
          if (flag)
          {
            int RequiredSize = 0;
            int PropertyRegDataType = 1;
            string str2 = "";
            if (USB.SetupDiGetDeviceRegistryProperty(classDevs, ref DeviceInfoData, 9, ref PropertyRegDataType, num1, 2048, ref RequiredSize))
              str2 = Marshal.PtrToStringAuto(num1);
            if (str2 == DriverKeyName)
            {
              int num2 = 2048;
              StringBuilder DeviceInstanceId = new StringBuilder(num2);
              USB.SetupDiGetDeviceInstanceId(classDevs, ref DeviceInfoData, DeviceInstanceId, num2, out RequiredSize);
              str1 = DeviceInstanceId.ToString();
              break;
            }
          }
          ++MemberIndex;
        }
        while (flag);
        Marshal.FreeHGlobal(num1);
        USB.SetupDiDestroyDeviceInfoList(classDevs);
      }
      return str1;
    }

    private struct STORAGE_DEVICE_NUMBER
    {
      public int DeviceType;
      public int DeviceNumber;
      public int PartitionNumber;
    }

    private enum USB_HUB_NODE
    {
      UsbHub,
      UsbMIParent,
    }

    private enum USB_CONNECTION_STATUS
    {
      NoDeviceConnected,
      DeviceConnected,
      DeviceFailedEnumeration,
      DeviceGeneralFailure,
      DeviceCausedOvercurrent,
      DeviceNotEnoughPower,
      DeviceNotEnoughBandwidth,
      DeviceHubNestedTooDeeply,
      DeviceInLegacyHub,
    }

    private enum USB_DEVICE_SPEED : byte
    {
      UsbLowSpeed,
      UsbFullSpeed,
      UsbHighSpeed,
    }

    private struct SP_DEVINFO_DATA
    {
      public int cbSize;
      public Guid ClassGuid;
      public int DevInst;
      public IntPtr Reserved;
    }

    private struct SP_DEVICE_INTERFACE_DATA
    {
      public int cbSize;
      public Guid InterfaceClassGuid;
      public int Flags;
      public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
      public int cbSize;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      public string DevicePath;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct USB_HCD_DRIVERKEY_NAME
    {
      public int ActualLength;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      public string DriverKeyName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct USB_ROOT_HUB_NAME
    {
      public int ActualLength;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      public string RootHubName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct USB_HUB_DESCRIPTOR
    {
      public byte bDescriptorLength;
      public byte bDescriptorType;
      public byte bNumberOfPorts;
      public short wHubCharacteristics;
      public byte bPowerOnToPowerGood;
      public byte bHubControlCurrent;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
      public byte[] bRemoveAndPowerMask;
    }

    private struct USB_HUB_INFORMATION
    {
      public USB.USB_HUB_DESCRIPTOR HubDescriptor;
      public byte HubIsBusPowered;
    }

    private struct USB_NODE_INFORMATION
    {
      public int NodeType;
      public USB.USB_HUB_INFORMATION HubInformation;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct USB_NODE_CONNECTION_INFORMATION_EX
    {
      public int ConnectionIndex;
      public USB.USB_DEVICE_DESCRIPTOR DeviceDescriptor;
      public byte CurrentConfigurationValue;
      public byte Speed;
      public byte DeviceIsHub;
      public short DeviceAddress;
      public int NumberOfOpenPipes;
      public int ConnectionStatus;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct USB_DEVICE_DESCRIPTOR
    {
      public byte bLength;
      public byte bDescriptorType;
      public short bcdUSB;
      public byte bDeviceClass;
      public byte bDeviceSubClass;
      public byte bDeviceProtocol;
      public byte bMaxPacketSize0;
      public short idVendor;
      public short idProduct;
      public short bcdDevice;
      public byte iManufacturer;
      public byte iProduct;
      public byte iSerialNumber;
      public byte bNumConfigurations;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct USB_STRING_DESCRIPTOR
    {
      public byte bLength;
      public byte bDescriptorType;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
      public string bString;
    }

    private struct USB_SETUP_PACKET
    {
      public byte bmRequest;
      public byte bRequest;
      public short wValue;
      public short wIndex;
      public short wLength;
    }

    private struct USB_DESCRIPTOR_REQUEST
    {
      public int ConnectionIndex;
      public USB.USB_SETUP_PACKET SetupPacket;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct USB_NODE_CONNECTION_NAME
    {
      public int ConnectionIndex;
      public int ActualLength;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      public string NodeName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct USB_NODE_CONNECTION_DRIVERKEY_NAME
    {
      public int ConnectionIndex;
      public int ActualLength;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
      public string DriverKeyName;
    }

    public class USBController
    {
      internal int ControllerIndex;
      internal string ControllerDriverKeyName;
      internal string ControllerDevicePath;
      internal string ControllerDeviceDesc;

      public USBController()
      {
        this.ControllerIndex = 0;
        this.ControllerDevicePath = "";
        this.ControllerDeviceDesc = "";
        this.ControllerDriverKeyName = "";
      }

      public int Index => this.ControllerIndex;

      public string DevicePath => this.ControllerDevicePath;

      public string DriverKeyName => this.ControllerDriverKeyName;

      public string Name => this.ControllerDeviceDesc;

      public USB.USBHub GetRootHub()
      {
        USB.USBHub usbHub = new USB.USBHub();
        usbHub.HubIsRootHub = true;
        usbHub.HubDeviceDesc = "Root Hub";
        IntPtr file1 = USB.CreateFile(this.ControllerDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
        if (file1.ToInt32() != -1)
        {
          int num1 = Marshal.SizeOf((object) new USB.USB_ROOT_HUB_NAME());
          IntPtr num2 = Marshal.AllocHGlobal(num1);
          int lpBytesReturned;
          if (USB.DeviceIoControl(file1, 2229256, num2, num1, num2, num1, out lpBytesReturned, IntPtr.Zero))
          {
            USB.USB_ROOT_HUB_NAME structure = (USB.USB_ROOT_HUB_NAME) Marshal.PtrToStructure(num2, typeof (USB.USB_ROOT_HUB_NAME));
            usbHub.HubDevicePath = "\\\\.\\" + structure.RootHubName;
          }
          IntPtr file2 = USB.CreateFile(usbHub.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
          if (file2.ToInt32() != -1)
          {
            USB.USB_NODE_INFORMATION usbNodeInformation = new USB.USB_NODE_INFORMATION();
            usbNodeInformation.NodeType = 0;
            int num3 = Marshal.SizeOf((object) usbNodeInformation);
            IntPtr num4 = Marshal.AllocHGlobal(num3);
            Marshal.StructureToPtr((object) usbNodeInformation, num4, true);
            if (USB.DeviceIoControl(file2, 2229256, num4, num3, num4, num3, out lpBytesReturned, IntPtr.Zero))
            {
              USB.USB_NODE_INFORMATION structure = (USB.USB_NODE_INFORMATION) Marshal.PtrToStructure(num4, typeof (USB.USB_NODE_INFORMATION));
              usbHub.HubIsBusPowered = Convert.ToBoolean(structure.HubInformation.HubIsBusPowered);
              usbHub.HubPortCount = (int) structure.HubInformation.HubDescriptor.bNumberOfPorts;
            }
            Marshal.FreeHGlobal(num4);
            USB.CloseHandle(file2);
          }
          Marshal.FreeHGlobal(num2);
          USB.CloseHandle(file1);
        }
        return usbHub;
      }
    }

    public class USBHub
    {
      internal int HubPortCount;
      internal string HubDriverKey;
      internal string HubDevicePath;
      internal string HubDeviceDesc;
      internal string HubManufacturer;
      internal string HubProduct;
      internal string HubSerialNumber;
      internal string HubInstanceID;
      internal bool HubIsBusPowered;
      internal bool HubIsRootHub;

      public USBHub()
      {
        this.HubPortCount = 0;
        this.HubDevicePath = "";
        this.HubDeviceDesc = "";
        this.HubDriverKey = "";
        this.HubIsBusPowered = false;
        this.HubIsRootHub = false;
        this.HubManufacturer = "";
        this.HubProduct = "";
        this.HubSerialNumber = "";
        this.HubInstanceID = "";
      }

      public int PortCount => this.HubPortCount;

      public string DevicePath => this.HubDevicePath;

      public string DriverKey => this.HubDriverKey;

      public string Name => this.HubDeviceDesc;

      public string InstanceID => this.HubInstanceID;

      public bool IsBusPowered => this.HubIsBusPowered;

      public bool IsRootHub => this.HubIsRootHub;

      public string Manufacturer => this.HubManufacturer;

      public string Product => this.HubProduct;

      public string SerialNumber => this.HubSerialNumber;

      public ReadOnlyCollection<USB.USBPort> GetPorts()
      {
        List<USB.USBPort> usbPortList = new List<USB.USBPort>();
        IntPtr file = USB.CreateFile(this.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
        if (file.ToInt32() != -1)
        {
          int num1 = Marshal.SizeOf(typeof (USB.USB_NODE_CONNECTION_INFORMATION_EX));
          IntPtr num2 = Marshal.AllocHGlobal(num1);
          for (int index = 1; index <= this.HubPortCount; ++index)
          {
            Marshal.StructureToPtr((object) new USB.USB_NODE_CONNECTION_INFORMATION_EX()
            {
              ConnectionIndex = index
            }, num2, true);
            if (USB.DeviceIoControl(file, 2229320, num2, num1, num2, num1, out int _, IntPtr.Zero))
            {
              USB.USB_NODE_CONNECTION_INFORMATION_EX structure = (USB.USB_NODE_CONNECTION_INFORMATION_EX) Marshal.PtrToStructure(num2, typeof (USB.USB_NODE_CONNECTION_INFORMATION_EX));
              USB.USBPort usbPort = new USB.USBPort();
              usbPort.PortPortNumber = index;
              usbPort.PortHubDevicePath = this.HubDevicePath;
              USB.USB_CONNECTION_STATUS connectionStatus = (USB.USB_CONNECTION_STATUS) structure.ConnectionStatus;
              usbPort.PortStatus = connectionStatus.ToString();
              USB.USB_DEVICE_SPEED speed = (USB.USB_DEVICE_SPEED) structure.Speed;
              usbPort.PortSpeed = speed.ToString();
              usbPort.PortIsDeviceConnected = structure.ConnectionStatus == 1;
              usbPort.PortIsHub = Convert.ToBoolean(structure.DeviceIsHub);
              usbPort.PortDeviceDescriptor = structure.DeviceDescriptor;
              usbPortList.Add(usbPort);
            }
          }
          Marshal.FreeHGlobal(num2);
          USB.CloseHandle(file);
        }
        return new ReadOnlyCollection<USB.USBPort>((IList<USB.USBPort>) usbPortList);
      }
    }

    public class USBPort
    {
      internal int PortPortNumber;
      internal string PortStatus;
      internal string PortHubDevicePath;
      internal string PortSpeed;
      internal bool PortIsHub;
      internal bool PortIsDeviceConnected;
      internal USB.USB_DEVICE_DESCRIPTOR PortDeviceDescriptor;

      public USBPort()
      {
        this.PortPortNumber = 0;
        this.PortStatus = "";
        this.PortHubDevicePath = "";
        this.PortSpeed = "";
        this.PortIsHub = false;
        this.PortIsDeviceConnected = false;
      }

      public int PortNumber => this.PortPortNumber;

      public string HubDevicePath => this.PortHubDevicePath;

      public string Status => this.PortStatus;

      public string Speed => this.PortSpeed;

      public bool IsHub => this.PortIsHub;

      public bool IsDeviceConnected => this.PortIsDeviceConnected;

      public USB.USBDevice GetDevice()
      {
        if (!this.PortIsDeviceConnected)
          return (USB.USBDevice) null;
        USB.USBDevice usbDevice = new USB.USBDevice();
        usbDevice.DevicePortNumber = this.PortPortNumber;
        usbDevice.DeviceHubDevicePath = this.PortHubDevicePath;
        usbDevice.DeviceDescriptor = this.PortDeviceDescriptor;
        IntPtr file = USB.CreateFile(this.PortHubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
        if (file.ToInt32() != -1)
        {
          int num1 = 2048;
          string s = new string(char.MinValue, 2048 / Marshal.SystemDefaultCharSize);
          int lpBytesReturned;
          if (this.PortDeviceDescriptor.iManufacturer > (byte) 0)
          {
            USB.USB_DESCRIPTOR_REQUEST descriptorRequest = new USB.USB_DESCRIPTOR_REQUEST()
            {
              ConnectionIndex = this.PortPortNumber,
              SetupPacket = {
                wValue = (short) (768 + (int) this.PortDeviceDescriptor.iManufacturer)
              }
            };
            descriptorRequest.SetupPacket.wLength = (short) (num1 - Marshal.SizeOf((object) descriptorRequest));
            descriptorRequest.SetupPacket.wIndex = (short) 1033;
            IntPtr hglobalAuto = Marshal.StringToHGlobalAuto(s);
            Marshal.StructureToPtr((object) descriptorRequest, hglobalAuto, true);
            if (USB.DeviceIoControl(file, 2229264, hglobalAuto, num1, hglobalAuto, num1, out lpBytesReturned, IntPtr.Zero))
            {
              USB.USB_STRING_DESCRIPTOR structure = (USB.USB_STRING_DESCRIPTOR) Marshal.PtrToStructure(new IntPtr(hglobalAuto.ToInt32() + Marshal.SizeOf((object) descriptorRequest)), typeof (USB.USB_STRING_DESCRIPTOR));
              usbDevice.DeviceManufacturer = structure.bString;
            }
            Marshal.FreeHGlobal(hglobalAuto);
          }
          if (this.PortDeviceDescriptor.iProduct > (byte) 0)
          {
            USB.USB_DESCRIPTOR_REQUEST descriptorRequest = new USB.USB_DESCRIPTOR_REQUEST()
            {
              ConnectionIndex = this.PortPortNumber,
              SetupPacket = {
                wValue = (short) (768 + (int) this.PortDeviceDescriptor.iProduct)
              }
            };
            descriptorRequest.SetupPacket.wLength = (short) (num1 - Marshal.SizeOf((object) descriptorRequest));
            descriptorRequest.SetupPacket.wIndex = (short) 1033;
            IntPtr hglobalAuto = Marshal.StringToHGlobalAuto(s);
            Marshal.StructureToPtr((object) descriptorRequest, hglobalAuto, true);
            if (USB.DeviceIoControl(file, 2229264, hglobalAuto, num1, hglobalAuto, num1, out lpBytesReturned, IntPtr.Zero))
            {
              USB.USB_STRING_DESCRIPTOR structure = (USB.USB_STRING_DESCRIPTOR) Marshal.PtrToStructure(new IntPtr(hglobalAuto.ToInt32() + Marshal.SizeOf((object) descriptorRequest)), typeof (USB.USB_STRING_DESCRIPTOR));
              usbDevice.DeviceProduct = structure.bString;
            }
            Marshal.FreeHGlobal(hglobalAuto);
          }
          if (this.PortDeviceDescriptor.iSerialNumber > (byte) 0)
          {
            USB.USB_DESCRIPTOR_REQUEST descriptorRequest = new USB.USB_DESCRIPTOR_REQUEST()
            {
              ConnectionIndex = this.PortPortNumber,
              SetupPacket = {
                wValue = (short) (768 + (int) this.PortDeviceDescriptor.iSerialNumber)
              }
            };
            descriptorRequest.SetupPacket.wLength = (short) (num1 - Marshal.SizeOf((object) descriptorRequest));
            descriptorRequest.SetupPacket.wIndex = (short) 1033;
            IntPtr hglobalAuto = Marshal.StringToHGlobalAuto(s);
            Marshal.StructureToPtr((object) descriptorRequest, hglobalAuto, true);
            if (USB.DeviceIoControl(file, 2229264, hglobalAuto, num1, hglobalAuto, num1, out lpBytesReturned, IntPtr.Zero))
            {
              USB.USB_STRING_DESCRIPTOR structure = (USB.USB_STRING_DESCRIPTOR) Marshal.PtrToStructure(new IntPtr(hglobalAuto.ToInt32() + Marshal.SizeOf((object) descriptorRequest)), typeof (USB.USB_STRING_DESCRIPTOR));
              usbDevice.DeviceSerialNumber = structure.bString;
            }
            Marshal.FreeHGlobal(hglobalAuto);
          }
          USB.USB_NODE_CONNECTION_DRIVERKEY_NAME connectionDriverkeyName = new USB.USB_NODE_CONNECTION_DRIVERKEY_NAME();
          connectionDriverkeyName.ConnectionIndex = this.PortPortNumber;
          int num2 = Marshal.SizeOf((object) connectionDriverkeyName);
          IntPtr num3 = Marshal.AllocHGlobal(num2);
          Marshal.StructureToPtr((object) connectionDriverkeyName, num3, true);
          if (USB.DeviceIoControl(file, 2229280, num3, num2, num3, num2, out lpBytesReturned, IntPtr.Zero))
          {
            USB.USB_NODE_CONNECTION_DRIVERKEY_NAME structure = (USB.USB_NODE_CONNECTION_DRIVERKEY_NAME) Marshal.PtrToStructure(num3, typeof (USB.USB_NODE_CONNECTION_DRIVERKEY_NAME));
            usbDevice.DeviceDriverKey = structure.DriverKeyName;
            usbDevice.DeviceName = USB.GetDescriptionByKeyName(usbDevice.DeviceDriverKey);
            usbDevice.DeviceInstanceID = USB.GetInstanceIDByKeyName(usbDevice.DeviceDriverKey);
          }
          Marshal.FreeHGlobal(num3);
          USB.CloseHandle(file);
        }
        return usbDevice;
      }

      public USB.USBHub GetHub()
      {
        if (!this.PortIsHub)
          return (USB.USBHub) null;
        USB.USBHub usbHub = new USB.USBHub();
        usbHub.HubIsRootHub = false;
        usbHub.HubDeviceDesc = "External Hub";
        IntPtr file1 = USB.CreateFile(this.PortHubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
        if (file1.ToInt32() != -1)
        {
          USB.USB_NODE_CONNECTION_NAME nodeConnectionName = new USB.USB_NODE_CONNECTION_NAME();
          nodeConnectionName.ConnectionIndex = this.PortPortNumber;
          int num1 = Marshal.SizeOf((object) nodeConnectionName);
          IntPtr num2 = Marshal.AllocHGlobal(num1);
          Marshal.StructureToPtr((object) nodeConnectionName, num2, true);
          int lpBytesReturned;
          if (USB.DeviceIoControl(file1, 2229268, num2, num1, num2, num1, out lpBytesReturned, IntPtr.Zero))
          {
            USB.USB_NODE_CONNECTION_NAME structure = (USB.USB_NODE_CONNECTION_NAME) Marshal.PtrToStructure(num2, typeof (USB.USB_NODE_CONNECTION_NAME));
            usbHub.HubDevicePath = "\\\\.\\" + structure.NodeName;
          }
          IntPtr file2 = USB.CreateFile(usbHub.HubDevicePath, 1073741824, 2, IntPtr.Zero, 3, 0, IntPtr.Zero);
          if (file2.ToInt32() != -1)
          {
            USB.USB_NODE_INFORMATION usbNodeInformation = new USB.USB_NODE_INFORMATION();
            usbNodeInformation.NodeType = 0;
            int num3 = Marshal.SizeOf((object) usbNodeInformation);
            IntPtr num4 = Marshal.AllocHGlobal(num3);
            Marshal.StructureToPtr((object) usbNodeInformation, num4, true);
            if (USB.DeviceIoControl(file2, 2229256, num4, num3, num4, num3, out lpBytesReturned, IntPtr.Zero))
            {
              USB.USB_NODE_INFORMATION structure = (USB.USB_NODE_INFORMATION) Marshal.PtrToStructure(num4, typeof (USB.USB_NODE_INFORMATION));
              usbHub.HubIsBusPowered = Convert.ToBoolean(structure.HubInformation.HubIsBusPowered);
              usbHub.HubPortCount = (int) structure.HubInformation.HubDescriptor.bNumberOfPorts;
            }
            Marshal.FreeHGlobal(num4);
            USB.CloseHandle(file2);
          }
          USB.USBDevice device = this.GetDevice();
          usbHub.HubInstanceID = device.DeviceInstanceID;
          usbHub.HubManufacturer = device.Manufacturer;
          usbHub.HubProduct = device.Product;
          usbHub.HubSerialNumber = device.SerialNumber;
          usbHub.HubDriverKey = device.DriverKey;
          Marshal.FreeHGlobal(num2);
          USB.CloseHandle(file1);
        }
        return usbHub;
      }
    }

    public class USBDevice
    {
      internal int DevicePortNumber;
      internal string DeviceDriverKey;
      internal string DeviceHubDevicePath;
      internal string DeviceInstanceID;
      internal string DeviceName;
      internal string DeviceManufacturer;
      internal string DeviceProduct;
      internal string DeviceSerialNumber;
      internal USB.USB_DEVICE_DESCRIPTOR DeviceDescriptor;

      public USBDevice()
      {
        this.DevicePortNumber = 0;
        this.DeviceHubDevicePath = "";
        this.DeviceDriverKey = "";
        this.DeviceManufacturer = "";
        this.DeviceProduct = "Unknown USB Device";
        this.DeviceSerialNumber = "";
        this.DeviceName = "";
        this.DeviceInstanceID = "";
      }

      public int PortNumber => this.DevicePortNumber;

      public string HubDevicePath => this.DeviceHubDevicePath;

      public string DriverKey => this.DeviceDriverKey;

      public string InstanceID => this.DeviceInstanceID;

      public string Name => this.DeviceName;

      public string Manufacturer => this.DeviceManufacturer;

      public string Product => this.DeviceProduct;

      public string SerialNumber => this.DeviceSerialNumber;
    }
  }
}
