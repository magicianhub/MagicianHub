using MagicianHub.Settings;
using System.Collections.ObjectModel;

namespace MagicianHub.Models
{
    public static class SavedAccounts
    {
        public static ObservableCollection<DataTypes.SavedAccounts> GetSavedAccounts()
        {
            ObservableCollection<DataTypes.SavedAccounts> savedAccounts =
                new ObservableCollection<DataTypes.SavedAccounts>();

            foreach (var savedAccount in SettingsManager.GetSettings().Auth.SavedAccounts)
            {
                savedAccounts.Add(new DataTypes.SavedAccounts
                {
                    Name = savedAccount.Name, Nickname = savedAccount.Nickname
                });
            }

            return savedAccounts;
        }
    }
}
