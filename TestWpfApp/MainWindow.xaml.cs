namespace TestWpfApp
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows;

    public partial class MainWindow : Window
    {
        public SecureString SecureString1 { get; set; } = new SecureString();

        public SecureString SecureString2 { get; set; } = new SecureString();

        public MainWindow()
        {
            InitializeComponent();

            SecureString1.InsertAt(0, '1');
            SecureString2.InsertAt(0, '2');

            Gg();
        }

        public bool Gg()
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;

            try
            {
                bstr1 = Marshal.SecureStringToBSTR(SecureString1);
                bstr2 = Marshal.SecureStringToBSTR(SecureString2);

                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);

                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);

                        if (b1 != b2)
                            return false;
                    }
                }
                else
                    return false;

                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(bstr1);
            }
        }
    }
}
