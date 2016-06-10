using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Slycoder.UI
{
    public class Popup
    {
        /// <summary>
        ///     Wrapper for the message dialog
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <param name="button1"></param>
        /// <param name="button2"></param>
        /// <returns>Returns true if the first button was pressed or false is the second button was pressed</returns>
        public static async Task<bool> Show(string caption, string message, string button1, string button2 = "")
        {
            var dialog = new MessageDialog(message, caption) {Commands = {new UICommand {Label = button1, Id = 0}}};
            IUICommand result = null;

            if (button2 != string.Empty)
            {
                dialog.Commands.Add(new UICommand {Label = button2, Id = 2});
            }

            await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                result = await dialog.ShowAsync();
            } );

            return result != null && result.Label == button1;
        }

        public static async Task<string> GetTextFromSpeech()
        {
            // Create an instance of SpeechRecognizer.
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            // Compile the dictation grammar by default.
            await speechRecognizer.CompileConstraintsAsync();

            // Start recognition.
            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();

            return speechRecognitionResult.Text;
        }
    }
}