namespace HybridFileXfer.Net.Models
{
    public class NetworkInfo : ObservableObject
    {

        public string NicName { get; set; }
        public string IPAddress { get; set; }

        private bool isSelected = false;

        public bool IsSelected
        {
            get => isSelected;
            set { isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public override string ToString()
        {
            return $"{NicName} {IPAddress}";
        }
    }
}
