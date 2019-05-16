using System;
using System.Windows.Forms;

namespace ChallengeRequireExample
{
    internal static class InstaProgram
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InstaForm1());
        }
    }
}