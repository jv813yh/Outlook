using Prism.Common;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.ComponentModel;
using System.Windows;

namespace Outlook.Wpf.Core.Dialogs
{

    /// <summary>
    /// I copied this code from the Prism library on github:
    /// https://github.com/PrismLibrary/Prism/blob/master/src/Wpf/Prism.Wpf/Dialogs/DialogService.cs
    /// </summary>
    public class DialogServiceBase : IDialogService
    {
        private readonly IContainerExtension _containerExtension;

        // Used to define and manipulate dialog windows
        protected IDialogWindow DialogWindow { get; private set; }

        public DialogServiceBase(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }
        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, false);
        }

        private void ShowDialogInternal(string name, IDialogParameters parameters, Action<IDialogResult> callback, bool isModal)
        {
            DialogWindow = CreateDialogWindow();
            ConfigureDialogWindowEvents(DialogWindow, callback);
            ConfigureDialogWindowContent(name, DialogWindow, parameters);

            //
            InitialDialogWindow(name, parameters);

            // TODO
            //DialogWindow.Initialize();

            if (isModal)
            {
                DialogWindow.ShowDialog();
            }
            else
            {
                DialogWindow.Show();
            }
        }

        protected virtual void InitialDialogWindow(string name, IDialogParameters parametrs)
        {
        }

        IDialogWindow CreateDialogWindow()
         => _containerExtension.Resolve<IDialogWindow>();

        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, true);
        }

        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {

        }

        protected virtual void ConfigureDialogWindowContent(string dialogName, IDialogWindow window, IDialogParameters parameters)
        {
            var content = _containerExtension.Resolve<object>(dialogName);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");


            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            ConfigureDialogWindowProperties(window, dialogContent, viewModel);

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(parameters));
        }

        protected virtual void ConfigureDialogWindowEvents(IDialogWindow dialogWindow, Action<IDialogResult> callback)
        {
            Action<IDialogResult> requestCloseHandler = (r) =>
            {
                dialogWindow.Result = r;
                dialogWindow.Close();
            };

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                dialogWindow.Loaded -= loadedHandler;
                dialogWindow.GetDialogViewModel().RequestClose += requestCloseHandler;
                //DialogUtilities.InitializeListener(dialogWindow.GetDialogViewModel(), requestCloseHandler);
            };
            dialogWindow.Loaded += loadedHandler;

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (!dialogWindow.GetDialogViewModel().CanCloseDialog())
                    e.Cancel = true;
            };
            dialogWindow.Closing += closingHandler;

            EventHandler closedHandler = null;
            closedHandler = async (o, e) =>
            {
                dialogWindow.Closed -= closedHandler;
                dialogWindow.Closing -= closingHandler;
                dialogWindow.GetDialogViewModel().RequestClose -= requestCloseHandler;

                dialogWindow.GetDialogViewModel().OnDialogClosed();

                if (dialogWindow.Result == null)
                    dialogWindow.Result = new DialogResult();

                callback?.Invoke(dialogWindow.Result);

                dialogWindow.DataContext = null;
                dialogWindow.Content = null;
            };
            dialogWindow.Closed += closedHandler;
        }

        protected virtual void ConfigureDialogWindowProperties(IDialogWindow window, FrameworkElement dialogContent, IDialogAware viewModel)
        {
            var windowStyle = Dialog.GetWindowStyle(dialogContent);
            if (windowStyle != null)
                window.Style = windowStyle;

            if (window.Content == null)
            {
                window.Content = dialogContent;
            }

            window.DataContext = viewModel; //we want the host window and the dialog to share the same data context

            if (window.Owner == null)
                window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        }
    }
    internal static class IDialogWindowExtensions
    {
        /// <summary>
        /// Get the <see cref="IDialogAware"/> ViewModel from a <see cref="IDialogWindow"/>.
        /// </summary>
        /// <param name="dialogWindow"><see cref="IDialogWindow"/> to get ViewModel from.</param>
        /// <returns>ViewModel as a <see cref="IDialogAware"/>.</returns>
        internal static IDialogAware GetDialogViewModel(this IDialogWindow dialogWindow)
        {
            return (IDialogAware)dialogWindow.DataContext;
        }
    }
}
