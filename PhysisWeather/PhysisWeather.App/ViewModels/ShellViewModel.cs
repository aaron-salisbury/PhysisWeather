using PhysisWeather.Core;
using PhysisWeather.Core.Base;
using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace PhysisWeather.App.ViewModels
{
    public class ShellViewModel : BaseNavigableViewModel
    {
        public static Visibility IsDebug
        {
#if DEBUG
            get { return Visibility.Visible; }
#else
            get { return Visibility.Collapsed; }
#endif
        }

        public AppLogger AppLogger { get; set; }
        public Manager Manager { get; set; }

        public ShellViewModel()
        {
            string applicationIdentifier = $"PhysisWeather-{GetDeviceIdentifier()}";

            AppLogger = new AppLogger();
            Manager = new Manager(AppLogger, applicationIdentifier);
        }

        private static string GetDeviceIdentifier()
        {
            EasClientDeviceInformation deviceInformation = new EasClientDeviceInformation();
            string deviceID = deviceInformation?.Id.ToString();

            if (!string.IsNullOrEmpty(deviceID))
            {
                return deviceID;
            }

            HardwareToken hardwareToken = HardwareIdentification.GetPackageSpecificToken(null);
            HashAlgorithmProvider hasher = HashAlgorithmProvider.OpenAlgorithm("MD5");
            IBuffer hashed = hasher.HashData(hardwareToken.Id);
            string hardwareID = CryptographicBuffer.EncodeToHexString(hashed);

            if (!string.IsNullOrEmpty(hardwareID))
            {
                return hardwareID;
            }

            return Guid.NewGuid().ToString();
        }
    }
}
