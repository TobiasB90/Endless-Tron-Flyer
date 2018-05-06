using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeerGamesLauncher.Helper
{
    public class NotifyPropertyChangedImpl : INotifyPropertyChanged
    {

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaiseAllPropertyChanged()
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(null));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}