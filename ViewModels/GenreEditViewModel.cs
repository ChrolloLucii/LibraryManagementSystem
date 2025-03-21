using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class GenreEditViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        private bool _isNew;
        
        public GenreEditViewModel(Genre genre = null, LibraryService libraryService = null)
        {
            _libraryService = libraryService ?? new LibraryService();
            _isNew = genre == null;
            Genre = genre ?? new Genre();
            
            SaveCommand = new RelayCommand(async _ => await Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }
        
        public Genre Genre { get; set; }
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
        public event Action RequestClose;
        
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Genre.Name);
        }
        
        private async Task Save()
        {
            try
            {
                if (_isNew)
                {
                    await _libraryService.AddGenreAsync(Genre);
                }
                else
                {
                    await _libraryService.UpdateGenreAsync(Genre);
                }
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении жанра: {ex.Message}\n\n{ex.InnerException?.Message}",
                              "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}