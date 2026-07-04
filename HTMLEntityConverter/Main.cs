using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;
using NppDemo.Utils;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "HTML Entity Converter";
        public static readonly string PluginConfigDirectory = Path.Combine(Npp.notepad.GetConfigDirectory(), PluginName);

        static internal void CommandMenuInit()
        {
            // Ensure dependencies load correctly from your subfolder
            AppDomain.CurrentDomain.AssemblyResolve += LoadDependency;

            // Register your custom HTML Entity Converter commands
            PluginBase.SetCommand(0, "Convert Selection to HTML Entities", ConvertToHtmlEntities, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(1, "Convert HTML Entities to Text", ConvertFromHtmlEntities, new ShortcutKey(false, false, false, Keys.None));
        }

        private static Assembly LoadDependency(object sender, ResolveEventArgs args)
        {
            string assemblyFile = Path.Combine(Npp.pluginDllDirectory, new AssemblyName(args.Name).Name) + ".dll";
            if (File.Exists(assemblyFile))
                return Assembly.LoadFrom(assemblyFile);
            return null;
        }

        static internal void SetToolBarIcons()
        {
            // Intentionally left blank.
        }

        public static void OnNotification(ScNotification notification)
        {
            // Intentionally left blank.
        }

        static internal void PluginCleanUp()
        {
            // Intentionally left blank.
        }

        /// <summary>
        /// Converts the currently selected text in the active Scintilla document to HTML entities.
        /// Retrieves the selection, encodes special characters using System.Net.WebUtility,
        /// and replaces the original selection with the encoded string. Warns the user if no text is selected.
        /// </summary>
        internal static void ConvertToHtmlEntities()
        {
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string rawText = scintilla.GetSelText();

            if (!string.IsNullOrEmpty(rawText))
            {
                string encodedText = System.Net.WebUtility.HtmlEncode(rawText);
                scintilla.ReplaceSel(encodedText);
            }
            else
            {
                MessageBox.Show("Please highlight text before using this command.", "No Selection Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Converts the currently selected HTML entities in the active Scintilla document back to standard text.
        /// Retrieves the selection, decodes special characters (including named entities like &amp;) using System.Net.WebUtility,
        /// and replaces the original selection with the decoded string. Warns the user if no text is selected.
        /// </summary>
        internal static void ConvertFromHtmlEntities()
        {
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            string rawText = scintilla.GetSelText();

            if (!string.IsNullOrEmpty(rawText))
            {
                string decodedText = System.Net.WebUtility.HtmlDecode(rawText);
                scintilla.ReplaceSel(decodedText);
            }
            else
            {
                MessageBox.Show("Please highlight text before using this command.", "No Selection Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}