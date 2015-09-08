using System.Security.Cryptography;
using altCrypt.Core.Encryption;
using altCrypt.Core.x86.Encryption;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

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
                SimpleIoc.Default.Register<IKey>(() => new Key("Pass@w0rd1")); //TODO accept input from user
                SimpleIoc.Default.Register<IIV, RandomIV>();
                SimpleIoc.Default.Register<SymmetricAlgorithm, AesCryptoServiceProvider>();
                SimpleIoc.Default.Register<IEncryptFile, StreamEncryptor>();
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