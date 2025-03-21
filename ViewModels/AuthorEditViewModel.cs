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
    public class AuthorEditViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        private bool _isNew;
        
        public AuthorEditViewModel(Author author = null, LibraryService libraryService = null)
        {
            _libraryService = libraryService ?? new LibraryService();
            _isNew = author == null;
            Author = author ?? new Author();
            
            SaveCommand = new RelayCommand(async _ => await Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }
        
        public Author Author { get; set; }
        
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        
        public event Action RequestClose;
        
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Author.FirstName) &&
                   !string.IsNullOrWhiteSpace(Author.LastName);
        }
        
        private async Task Save()
{
    try
    {
       
        if (string.IsNullOrWhiteSpace(Author.FirstName) || string.IsNullOrWhiteSpace(Author.LastName))
        {
            MessageBox.Show("Поля Имя и Фамилия обязательны.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        if (Author.BirthDate.HasValue)
        {
            DateTime localDate = Author.BirthDate.Value;
            Author.BirthDate = DateTime.SpecifyKind(localDate, DateTimeKind.Utc);
        }
        
        if (_isNew)
        {
            await _libraryService.AddAuthorAsync(Author);
        }
        else
        {
            await _libraryService.UpdateAuthorAsync(Author);
        }
        RequestClose?.Invoke();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Ошибка при сохранении автора: {ex.Message}\n\n{ex.InnerException?.Message}",
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