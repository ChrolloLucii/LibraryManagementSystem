using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace LibraryManagementSystem.ViewModels
{
    public class BookEditViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        private bool _isNew;
        
        public BookEditViewModel(Book book = null, LibraryService libraryService = null)
        {
            _libraryService = libraryService ?? new LibraryService();
            _isNew = book == null;
            Book = book ?? new Book();
            WindowTitle = _isNew ? "Добавление новой книги" : "Редактирование книги";
            
            SaveCommand = new RelayCommand(async _ => await Save(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
            
            Task.Run(async () => await LoadDataAsync()).Wait();
        }
        
        private async Task LoadDataAsync()
        {
            try
            {
                Authors = await _libraryService.GetAllAuthorsAsync();
                Genres = await _libraryService.GetAllGenresAsync();
                
                if (!_isNew)
                {
                    SelectedAuthor = Authors.FirstOrDefault(a => a.Id == Book.AuthorId);
                    SelectedGenre = Genres.FirstOrDefault(g => g.Id == Book.GenreId);
                }
                else if (Authors.Any() && Genres.Any())
                {
                    SelectedAuthor = Authors.First();
                    SelectedGenre = Genres.First();
                }
                
                OnPropertyChanged(nameof(Authors));
                OnPropertyChanged(nameof(Genres));
                OnPropertyChanged(nameof(SelectedAuthor));
                OnPropertyChanged(nameof(SelectedGenre));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LoadData()
        {
            Task.Run(async () => await LoadDataAsync()).Wait();
        }
        
        public event Action RequestClose;
        
        public string WindowTitle { get; private set; }
        
        public Book Book { get; set; }
        
        public IEnumerable<Author> Authors { get; private set; }
        
        public IEnumerable<Genre> Genres { get; private set; }
        
        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                if (value != null)
                {
                    Book.AuthorId = value.Id;
                    Book.Author = value;
                }
                OnPropertyChanged();
            }
        }
        
        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                if (value != null)
                {
                    Book.GenreId = value.Id;
                    Book.Genre = value;
                }
                OnPropertyChanged();
            }
        }
        
        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        
        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Book.Title) &&
                   Book.PublishYear > 0 &&
                   Book.QuantityInStock >= 0 &&
                   SelectedAuthor != null &&
                   SelectedGenre != null;
        }
        
        private async Task Save()
        {
            try
            {
                if (_isNew)
                {
                    await _libraryService.AddBookAsync(Book);
                }
                else
                {
                    await _libraryService.UpdateBookAsync(Book);
                }
                
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении книги: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}