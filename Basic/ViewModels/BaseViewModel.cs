
using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using System.Threading;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Basic.ViewModels
{
    public class BaseViewModel : Conductor<object>
    {
        private SemaphoreSlim AsyncLock = new SemaphoreSlim(1);
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;

        public BaseViewModel()
        {


            _eventAggregator = IoC.Get<IEventAggregator>();

        }


        protected override async void OnInitialize()
        {
            base.OnInitialize();

        }
        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);

            // Background Run.
            Task.Run(MainLoop);

        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);

        }
        private async Task MainLoop()
        {
            while (true)
            {
                Counter = _counter + 1;
                await Task.Delay(500);
                await ImportantNoStopJob();
            }
        }
        private async Task ImportantNoStopJob() // Important. I do not want to stop it during suspend.
        {
            Temp = _temp  - 1;
            await Task.Delay(500);
        }




        #region Properties
        private int _counter = 0;
        public int Counter
        {
            get
            {
                return _counter;
            }
            set
            {
                this.Set(ref _counter, value);
            }
        }
        private int _temp = 0;
        public int Temp
        {
            get
            {
                return _temp;
            }
            set
            {
                this.Set(ref _temp, value);
            }
        }
        #endregion
    }
}