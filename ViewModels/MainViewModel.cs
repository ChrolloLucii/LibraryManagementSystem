
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using LibraryManagementSystem.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagementSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;

        public MainViewModel()
        {
            _libraryService = new LibraryService();
            SearchQuery = string.Empty;
            AddBookCommand = new RelayCommand(_ => AddBook());
            EditBookCommand = new RelayCommand(_ => EditBook(), _ => SelectedBook != null);
            DeleteBookCommand = new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
            ManageAuthorsCommand = new RelayCommand(_ => ManageAuthors());
            ManageGenresCommand = new RelayCommand(_ => ManageGenres());

            LoadData();
        }

        private async void LoadData()
        {
            var books = await _libraryService.GetAllBooksAsync();
            Books = [.. books];
            FilteredBooks = new ObservableCollection<Book>(books);

            Authors = [.. await _libraryService.GetAllAuthorsAsync()];
            Genres = [.. await _libraryService.GetAllGenresAsync()];

            Authors.Insert(0, new Author { Id = 0, FirstName = "Все", LastName = "авторы" });
            Genres.Insert(0, new Genre { Id = 0, Name = "Все жанры" });

            SelectedAuthorFilter = Authors[0];
            SelectedGenreFilter = Genres[0];
        }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Book> _filteredBooks;
        public ObservableCollection<Book> FilteredBooks
        {
            get { return _filteredBooks; }
            set
            {
                _filteredBooks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        private Book _selectedBook;
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private Author _selectedAuthorFilter;
        public Author SelectedAuthorFilter
        {
            get { return _selectedAuthorFilter; }
            set
            {
                _selectedAuthorFilter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private Genre _selectedGenreFilter;
        public Genre SelectedGenreFilter
        {
            get { return _selectedGenreFilter; }
            set
            {
                _selectedGenreFilter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private string _searchQuery;
public string SearchQuery
{
    get { return _searchQuery; }
    set 
    { 
        _searchQuery = value; 
        OnPropertyChanged();
        ApplyFilter();
    }
}
        private void ApplyFilter()
{

    if (Books == null)
        return;
        
    var filtered = Books.AsEnumerable();


    if (SelectedAuthorFilter != null && SelectedAuthorFilter.Id != 0)
    {
        filtered = filtered.Where(b => b.AuthorId == SelectedAuthorFilter.Id);
    }


    if (SelectedGenreFilter != null && SelectedGenreFilter.Id != 0)
    {
        filtered = filtered.Where(b => b.GenreId == SelectedGenreFilter.Id);
    }


    if (!string.IsNullOrWhiteSpace(SearchQuery))
    {
        filtered = filtered.Where(b => b.Title != null && b.Title.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    FilteredBooks = new ObservableCollection<Book>(filtered);
}

        public ICommand AddBookCommand { get; private set; }
        public ICommand EditBookCommand { get; private set; }
        public ICommand DeleteBookCommand { get; private set; }
        public ICommand ManageAuthorsCommand { get; private set; }
public ICommand ManageGenresCommand { get; private set; }
        private void AddBook()
        {
            var viewModel = new BookEditViewModel(null, _libraryService);
            var window = new BookEditWindow(viewModel);

            if (window.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void EditBook()
        {
            if (SelectedBook != null)
            {
                var bookCopy = new Book
                {
                    Id = SelectedBook.Id,
                    Title = SelectedBook.Title,
                    ISBN = SelectedBook.ISBN,
                    PublishYear = SelectedBook.PublishYear,
                    QuantityInStock = SelectedBook.QuantityInStock,
                    AuthorId = SelectedBook.AuthorId,
                    GenreId = SelectedBook.GenreId
                };

                var viewModel = new BookEditViewModel(bookCopy, _libraryService);
                var window = new BookEditWindow(viewModel);

                if (window.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private async void ManageAuthors()
        {
            var viewModel = new AuthorManagementViewModel(_libraryService);
            var window = new AuthorManagementWindow(viewModel);

            if (window.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private void ManageGenres()
{
    var viewModel = new GenreManagementViewModel(_libraryService);
    var window = new GenreManagementWindow(viewModel);

    if (window.ShowDialog() == true)
    {
        LoadData();
    }
}
        private async void DeleteBook()
{
    if (SelectedBook != null)
    {
        if (MessageBox.Show("Вы ряльно хотите удалить книгу?", "Подтверждение удаления", 
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            try
            {
                await _libraryService.DeleteBookAsync(SelectedBook.Id);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении книги: {ex.Message}");
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