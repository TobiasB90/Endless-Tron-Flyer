using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using DeerGamesCommonLibrary.Helper;
using Microsoft.Win32;


namespace DeerGamesCommonLibrary.Services
{
    /// <summary>
    /// The configuration service provides the functionality to store data in the registry and retrieve it.
    /// </summary>
    public class ConfigurationService
    {
        #region private fields

        private const string DeerGamesRegKey = @"HKEY_CURRENT_USER\Software\DeerGames\Config";

        private static readonly Lazy<ConfigurationService> InternalInstance = new Lazy<ConfigurationService>(() => new ConfigurationService());

        private readonly byte[] entropy = { 12, 1, 15, 19, 17, 13, 1, 9, 2, 4, 13, 7, 14, 16, 2, 4, 6, 9, 12, 13 };

        #endregion

        #region Constructor

        private ConfigurationService()
        {
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets the instance of the configuration service.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ConfigurationService Instance => InternalInstance.Value;

        /// <summary>
        /// Gets or sets the import path. This will be used from the File browser dialog.
        /// </summary>
        /// <value>
        /// The import path.
        /// </value>
        public bool CredentialsSaved
        {
            get
            {
                return this.Password != null && this.Password.Length > 0 && !string.IsNullOrEmpty(this.Username);
            }
        }

        /// <summary>
        /// Gets or sets the export path. This will be used from the File browser dialog.
        /// </summary>
        /// <value>
        /// The export path.
        /// </value>
        public SecureString Password
        {
            get
            {
                return this.ConvertEncryptedStringToSecureString(this.ReadRegistryValue(DeerGamesRegKey, "Password"));
            }

            set
            {
                this.SaveRegistryValue(DeerGamesRegKey, "Password", this.ConvertSecureStringToEncryptedString(value));
            }
        }

        public string Username
        {
            get
            {
                return this.ReadRegistryValue<string>(DeerGamesRegKey, "Username");
            }

            set
            {
                this.SaveRegistryValue(DeerGamesRegKey, "Username", value);
            }
        }

        public string Token
        {
            get
            {
                return this.ReadRegistryValue<string>(DeerGamesRegKey, "Token");
            }

            set
            {
                this.SaveRegistryValue(DeerGamesRegKey, "Token", value);
            }
        }

        #endregion

        #region private methods

        private static RegistryKey GetRegistryFromKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }

            var regKey = new RegistryKey();

            var rootString = key;
            if (key.Contains("\\"))
            {
                var split = key.Trim('\\').Split(new[] { '\\' }, 2);
                rootString = split.FirstOrDefault();

                if (split.Length > 1)
                {
                    regKey.Key = split.LastOrDefault();
                }
            }

            if (!string.IsNullOrWhiteSpace(rootString))
            {
                switch (rootString.ToUpper())
                {
                    case "HKEY_CURRENT_USER":
                    case "HKCU":
                        regKey.Root = RegistryHive.CurrentUser;
                        break;
                    case "HKEY_CLASSES_ROOT":
                    case "HKCR":
                        regKey.Root = RegistryHive.ClassesRoot;
                        break;
                    case "HKEY_CURRENT_CONFIG":
                    case "HKCC":
                        regKey.Root = RegistryHive.CurrentConfig;
                        break;
                    case "HKEY_USERS":
                    case "HKU":
                        regKey.Root = RegistryHive.Users;
                        break;
                    case "HKEY_LOCAL_MACHINE":
                    case "HKLM":
                        regKey.Root = RegistryHive.LocalMachine;
                        break;
                    case "HKEY_PERFORMANCE_DATA":
                        regKey.Root = RegistryHive.PerformanceData;
                        break;
                    case "HKEY_DYN_DATA":
                        regKey.Root = RegistryHive.DynData;
                        break;
                }
            }

            return regKey;
        }

        private T ReadRegistryValue<T>(string key, string name)
        {
            object regValue = null;
            var reg = GetRegistryFromKey(key);

            switch (reg.Root)
            {
                case RegistryHive.ClassesRoot:
                    var classesRootReg = Registry.ClassesRoot.OpenSubKey(reg.Key);
                    regValue = classesRootReg?.GetValue(name);
                    break;
                case RegistryHive.LocalMachine:
                    var localMachineRootReg = Registry.LocalMachine.OpenSubKey(reg.Key);
                    regValue = localMachineRootReg?.GetValue(name);
                    break;
                case RegistryHive.CurrentUser:
                    var currentUserReg = Registry.CurrentUser.OpenSubKey(reg.Key);
                    regValue = currentUserReg?.GetValue(name);
                    break;
                case RegistryHive.Users:
                    var usersReg = Registry.Users.OpenSubKey(reg.Key);
                    regValue = usersReg?.GetValue(name);
                    break;
                case RegistryHive.CurrentConfig:
                    var currentConfigReg = Registry.CurrentConfig.OpenSubKey(reg.Key);
                    regValue = currentConfigReg?.GetValue(name);
                    break;
            }

            return regValue != null ? (T)Convert.ChangeType(regValue, Type.GetTypeCode(typeof(T))) : default(T);
        }

        private byte[] ReadRegistryValue(string key, string name)
        {
            object regValue = null;
            var reg = GetRegistryFromKey(key);

            switch (reg.Root)
            {
                case RegistryHive.ClassesRoot:
                    var classesRootReg = Registry.ClassesRoot.OpenSubKey(reg.Key);
                    regValue = classesRootReg?.GetValue(name);
                    break;
                case RegistryHive.LocalMachine:
                    var localMachineRootReg = Registry.LocalMachine.OpenSubKey(reg.Key);
                    regValue = localMachineRootReg?.GetValue(name);
                    break;
                case RegistryHive.CurrentUser:
                    var currentUserReg = Registry.CurrentUser.OpenSubKey(reg.Key);
                    regValue = currentUserReg?.GetValue(name);
                    break;
                case RegistryHive.Users:
                    var usersReg = Registry.Users.OpenSubKey(reg.Key);
                    regValue = usersReg?.GetValue(name);
                    break;
                case RegistryHive.CurrentConfig:
                    var currentConfigReg = Registry.CurrentConfig.OpenSubKey(reg.Key);
                    regValue = currentConfigReg?.GetValue(name);
                    break;
            }

            return (byte[])regValue;
        }

        private void SaveRegistryValue(string key, string name, object value)
        {
            var reg = GetRegistryFromKey(key);

            switch (reg.Root)
            {
                case RegistryHive.ClassesRoot:
                    var classesRootReg = Registry.ClassesRoot.OpenSubKey(
                        reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (classesRootReg != null)
                    {
                        classesRootReg.SetValue(name, value);
                    }
                    else
                    {
                        classesRootReg = Registry.ClassesRoot.CreateSubKey(reg.Key,
                            RegistryKeyPermissionCheck.ReadWriteSubTree);
                        classesRootReg?.SetValue(name, value);
                    }

                    break;
                case RegistryHive.LocalMachine:
                    var localMachineReg = Registry.LocalMachine.OpenSubKey(
                        reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (localMachineReg != null)
                    {
                        localMachineReg.SetValue(name, value);
                    }
                    else
                    {
                        localMachineReg = Registry.LocalMachine.CreateSubKey(reg.Key,
                            RegistryKeyPermissionCheck.ReadWriteSubTree);
                        localMachineReg?.SetValue(name, value);
                    }

                    break;
                case RegistryHive.CurrentUser:
                    var currentUserReg = Registry.CurrentUser.OpenSubKey(
                        reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (currentUserReg != null)
                    {
                        currentUserReg.SetValue(name, value);
                    }
                    else
                    {
                        currentUserReg = Registry.CurrentUser.CreateSubKey(reg.Key,
                            RegistryKeyPermissionCheck.ReadWriteSubTree);
                        currentUserReg?.SetValue(name, value);
                    }

                    break;
                case RegistryHive.Users:
                    var usersReg = Registry.Users.OpenSubKey(reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (usersReg != null)
                    {
                        usersReg.SetValue(name, value);
                    }
                    else
                    {
                        usersReg = Registry.Users.CreateSubKey(reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        usersReg?.SetValue(name, value);
                    }

                    break;
                case RegistryHive.CurrentConfig:
                    var currentConfigReg = Registry.CurrentConfig.OpenSubKey(
                        reg.Key, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    if (currentConfigReg != null)
                    {
                        currentConfigReg.SetValue(name, value);
                    }
                    else
                    {
                        currentConfigReg = Registry.CurrentConfig.CreateSubKey(reg.Key,
                            RegistryKeyPermissionCheck.ReadWriteSubTree);
                        currentConfigReg?.SetValue(name, value);
                    }

                    break;
            }
        }

        private byte[] ConvertSecureStringToEncryptedString(SecureString passwordString)
        {
            try
            {
                IntPtr bstr = Marshal.SecureStringToBSTR(passwordString);
                string clearPw;

                try
                {
                    clearPw = Marshal.PtrToStringBSTR(bstr);
                }
                catch (Exception)
                {
                    clearPw = string.Empty;
                }
                finally
                {
                    Marshal.FreeBSTR(bstr);
                }

                byte[] plaintext = Encoding.UTF8.GetBytes(clearPw);

                // ReSharper disable once RedundantAssignment because it's just to remove the old clearpw object
                clearPw = string.Empty;

                byte[] ciphertext = ProtectedData.Protect(plaintext, this.entropy, DataProtectionScope.LocalMachine);

                // ReSharper disable once RedundantAssignment because it's just to remove the old clearpw bytearray
                plaintext = new byte[20];
                return ciphertext;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private SecureString ConvertEncryptedStringToSecureString(byte[] passwordString)
        {
            try
            {
                byte[] plaintext = ProtectedData.Unprotect(
                    passwordString,
                    this.entropy,
                    DataProtectionScope.LocalMachine);
                var securestring = SecureStringHelper.StringToSecureString(Encoding.UTF8.GetString(plaintext));

                // ReSharper disable once RedundantAssignment because it's just to remove the old clearpw bytearray
                plaintext = new byte[20];
                return securestring;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region private classes

        private class RegistryKey
        {
            public RegistryHive Root;
            public string Key;
        }

        #endregion
    }
}
