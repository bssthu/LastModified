// 
// Author       : bss
// Creation Date: 2014-12-30
// Last modified: 2014-12-31, 00:42:47
// Description  : Add last modified time on save.
// 

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace bssthu.LastModified
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.guidLastModifiedPkgString)]

    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    public sealed class LastModifiedPackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public LastModifiedPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        EnvDTE80.DTE2 m_DTE;
        EnvDTE80.Events2 m_saveEvents;
        private EnvDTE.CommandEvents save_CommandEvents;
        public EnvDTE.CommandEvents CommendEvents
        {
            get
            {
                return save_CommandEvents;
            }
            set
            {
                if (save_CommandEvents != null)
                {
                    save_CommandEvents.BeforeExecute -= CommandEvents_BeforeExecute;
                }
                save_CommandEvents = value;
                if (save_CommandEvents != null)
                {
                    save_CommandEvents.BeforeExecute += CommandEvents_BeforeExecute;
                }
            }
        }

        private void CommandEvents_BeforeExecute(string Guid, int ID, object CustomIn, object CustomOut, ref bool CancelDefault)
        {
            var selection = m_DTE.ActiveDocument.Selection as EnvDTE.TextSelection;
            int selBegin = selection.ActivePoint.AbsoluteCharOffset;
            selection.StartOfDocument();
            selection.LineDown(true, 20);
            String originText = selection.Text;
            String text = originText;
            DateTime date = DateTime.Now;
            String dateStr = date.ToString("yyyy-MM-dd, HH:mm:ss");
            selection.ReplacePattern("Last modified: .*\n?", "Last modified: " + dateStr + "\r\n",
                (int)EnvDTE.vsFindOptions.vsFindOptionsRegularExpression);
            selection.MoveToAbsoluteOffset(selBegin);
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            // custom
            m_DTE = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            m_saveEvents = m_DTE.Events as EnvDTE80.Events2;
            CommendEvents = m_saveEvents.CommandEvents["{5EFC7975-14BC-11CF-9B2B-00AA00573819}", 331];
        }
        #endregion

    }
}
