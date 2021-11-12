// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System.Linq;

using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Commands
{
    internal abstract class NoDevicesCommandBase : DisabledCommand
    {
        private readonly AudioDeviceViewModelManager _viewModelManager;
        private readonly AudioDeviceKind _kind;

        public NoDevicesCommandBase(string text, AudioDeviceViewModelManager viewModelManager, AudioDeviceKind kind)
            : base(text)
        {
            _kind = kind;
            _viewModelManager = viewModelManager;
        }

        public override void Refresh()
        {
            IsVisible = !AnyVisibleDevices();
        }

        private bool AnyVisibleDevices()
        {
            return _viewModelManager.ViewModels.Where(vm => vm.Kind == _kind && vm.IsVisible)
                                               .Any();
        }
    }
}
