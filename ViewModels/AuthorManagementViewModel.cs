using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class AuthorManagementViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        
        public AuthorManagementViewModel(LibraryService libraryService = null)
        {
            _libraryService = libraryService ?? new LibraryService();
            Authors = new ObservableCollection<Author>();
            AddAuthorCommand = new RelayCommand(_ => AddAuthor());
            EditAuthorCommand = new RelayCommand(_ => EditAuthor(), _ => SelectedAuthor != null);
            DeleteAuthorCommand = new RelayCommand(async _ => await DeleteAuthor(), _ => SelectedAuthor != null);
            LoadAuthors();
        }
        
        private async void LoadAuthors()
        {
            var authors = await _libraryService.GetAllAuthorsAsync();
            Authors = new ObservableCollection<Author>(authors);
        }
        
        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set { _authors = value; OnPropertyChanged(); }
        }
        
        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set { _selectedAuthor = value; OnPropertyChanged(); }
        }
        
        public ICommand AddAuthorCommand { get; }
        public ICommand EditAuthorCommand { get; }
        public ICommand DeleteAuthorCommand { get; }
        
        private void AddAuthor()
        {
            var viewModel = new AuthorEditViewModel(null, _libraryService);
            var window = new AuthorEditWindow(viewModel);
            if (window.ShowDialog() == true)
            {
                LoadAuthors();
            }
        }
        
        private void EditAuthor()
        {
            if (SelectedAuthor != null)
            {
                var copy = new Author
                {
                    Id = SelectedAuthor.Id,
                    FirstName = SelectedAuthor.FirstName,
                    LastName = SelectedAuthor.LastName,
                    Country = SelectedAuthor.Country,
                    BirthDate = SelectedAuthor.BirthDate
                };
                
                var viewModel = new AuthorEditViewModel(copy, _libraryService);
                var window = new AuthorEditWindow(viewModel);
                if (window.ShowDialog() == true)
                {
                    LoadAuthors();
                }
            }
        }
        
        private async Task DeleteAuthor()
        {
            if (SelectedAuthor != null)
            {
                if (MessageBox.Show("Вы рялно хотите удалить автора?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _libraryService.DeleteAuthorAsync(SelectedAuthor.Id);
                        LoadAuthors();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}