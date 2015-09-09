using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.x86.Encryption;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using altCrypt.Business;

namespace altCrypt.Client.Desktop.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                //TODO
            }
            else
            {
                string fileExtension = ".altCrypt";
                SimpleIoc.Default.Register<IKey>(() => new Key("Pass@w0rd1")); //TODO accept input from user
                SimpleIoc.Default.Register<IIV, RandomIV>();
                SimpleIoc.Default.Register<SymmetricAlgorithm, AesCryptoServiceProvider>();
                SimpleIoc.Default.Register<IEncryptFiles, StreamEncryptor>();

                var encryptor = SimpleIoc.Default.GetInstance<IEncryptFiles>();
                SimpleIoc.Default.Register<IFileProcessor>(() => new FileProcessor(fileExtension, encryptor));
            }

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}