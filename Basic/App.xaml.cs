using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

using Windows.UI.Core;
using Caliburn.Micro;
using Basic.ViewModels;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using Windows.UI.Notifications;

namespace Basic
{


    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>

    sealed partial class App : CaliburnApplication
    {
        private WinRTContainer _container;



        public App()
        {

            this.InitializeComponent();

        }

        protected override void Configure()
        {
           
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();


            // Make sure to register your containers here
            _container
                // Base
                .PerRequest<BaseViewModel>()
                ;
            

        }

        /// < summary >
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>

        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            // Should I insert here ? ( A )
            /* var extendedExecutionSession = new ExtendedExecutionSession();
             extendedExecutionSession.Reason = ExtendedExecutionReason.Unspecified;
             var extendedExecutionResult = await extendedExecutionSession.RequestExtensionAsync();
             if (extendedExecutionResult != ExtendedExecutionResult.Allowed)
             {
                 ShowToast("Extended execution session request: denied");

                 //extended execution session revoked
                 extendedExecutionSession.Dispose();
                 extendedExecutionSession = null;
             }
             */


            // Should I insert here ? ( B )
            /*
            var newSession = new ExtendedExecutionForegroundSession();
            newSession.Reason = ExtendedExecutionForegroundReason.Unconstrained;
            newSession.Description = "Long Running Processing";
            newSession.Revoked += SessionRevoked;
            var result = await newSession.RequestExtensionAsync();
            switch (result)
            {
                case ExtendedExecutionForegroundResult.Allowed:
                    ShowToast($"ExtendedExecutionForegroundResult.Allowed");
                    break;

                default:
                case ExtendedExecutionForegroundResult.Denied:
                    ShowToast($"ExtendedExecutionForegroundResult.Denied");
                    break;
            }
            */

            if (e.PreviousExecutionState == ApplicationExecutionState.Running)
                return;

            DisplayRootViewFor<BaseViewModel>();

        }
        private void SessionRevoked(object sender, ExtendedExecutionForegroundRevokedEventArgs args)
        {
            ShowToast("Extended execution session revoked: " + args.Reason.ToString());
        }
        private void ExtensionRevoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            ShowToast("ExtensionRevoked: " + args.Reason.ToString());
        }
        private void ShowToast(string text)
        {
            Debug.WriteLine(text);
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].InnerText = text;
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
        }
        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            ShowToast("OnSuspending ...");

            var deferral = e.SuspendingOperation.GetDeferral();

            // Should I insert here ? ( C )
            /*
            using (var session = new ExtendedExecutionSession())
            {

                session.Reason = ExtendedExecutionReason.Unspecified;
                session.Description = "Saving Data to cloud";
                session.Revoked += ExtensionRevoked;
                var result = await session.RequestExtensionAsync();
                if (result == ExtendedExecutionResult.Allowed)
                {
                    return; // How to stop suspend forever ??????
                }

            }*/


            // Should I insert here ? ( D )
            /*
            var newSession = new ExtendedExecutionForegroundSession();
            newSession.Reason = ExtendedExecutionForegroundReason.Unconstrained;
            newSession.Description = "Long Running Processing";
            newSession.Revoked += SessionRevoked;
            var result = await newSession.RequestExtensionAsync();
            switch (result)
            {
                case ExtendedExecutionForegroundResult.Allowed:
                    ShowToast($"ExtendedExecutionForegroundResult.Allowed");
                    break;

                default:
                case ExtendedExecutionForegroundResult.Denied:
                    ShowToast($"ExtendedExecutionForegroundResult.Denied");
                    break;
            }
            */

            deferral.Complete();


        }

        

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            var navigationService = _container.RegisterNavigationService(rootFrame);
            var navigationManager = SystemNavigationManager.GetForCurrentView();

            navigationService.Navigated += (s, e) =>
            {
                navigationManager.AppViewBackButtonVisibility = navigationService.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;
            };

        }

        protected override object GetInstance(Type service, string key)
        {

            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {

            _container.BuildUp(instance);
        }

    }


}
