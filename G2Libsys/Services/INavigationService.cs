﻿namespace G2Libsys.Services
{
    using G2Libsys.ViewModels;
    using System;
    using System.Collections.Generic;

    public interface INavigationService
    {
        /// <summary>
        /// Main viewmodel
        /// </summary>
        IHostScreen HostScreen { get; }

        /// <summary>
        /// Viewmodel navigation stack
        /// </summary>
        IList<IViewModel> ViewModels { get; }

        /// <summary>
        /// NavService setup where vm is HostScreen
        /// </summary>
        /// <param name="vm">HostScreen</param>
        void Setup(IHostScreen vm);

        /// <summary>
        /// Look for viewmodel in navigationstack
        /// </summary>
        /// <param name="vm">Viewmodel Type</param>
        IViewModel Locate(Type vm);

        /// <summary>
        /// Return viewmodel from stack or add new to stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        IViewModel GetViewModel(IViewModel vm);

        /// <summary>
        /// Set main viewmodel and clear stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        void GoToAndReset(IViewModel vm);

        /// <summary>
        /// Return new instance of the VM and remove previous from stack
        /// </summary>
        /// <param name="vm">Viewmodel to navigate to</param>
        IViewModel CreateNewInstance(IViewModel vm);
    }
}
