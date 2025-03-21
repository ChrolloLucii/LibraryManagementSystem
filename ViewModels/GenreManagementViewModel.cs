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
    public class GenreManagementViewModel : INotifyPropertyChanged
    {
        private readonly LibraryService _libraryService;
        
        public GenreManagementViewModel(LibraryService libraryService = null)
        {
            _libraryService = libraryService ?? new LibraryService();
            Genres = new ObservableCollection<Genre>();
            AddGenreCommand = new RelayCommand(_ => AddGenre());
            EditGenreCommand = new RelayCommand(_ => EditGenre(), _ => SelectedGenre != null);
            DeleteGenreCommand = new RelayCommand(async _ => await DeleteGenre(), _ => SelectedGenre != null);
            LoadGenres();
        }
        
        private async void LoadGenres()
        {
            var genres = await _libraryService.GetAllGenresAsync();
            Genres = new ObservableCollection<Genre>(genres);
        }
        
        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set { _genres = value; OnPropertyChanged(); }
        }
        
        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set { _selectedGenre = value; OnPropertyChanged(); }
        }
        
        public ICommand AddGenreCommand { get; }
        public ICommand EditGenreCommand { get; }
        public ICommand DeleteGenreCommand { get; }
        
        private void AddGenre()
        {
            var viewModel = new GenreEditViewModel(null, _libraryService);
            var window = new GenreEditWindow(viewModel);
            if (window.ShowDialog() == true)
            {
                LoadGenres();
            }
        }
        
        private void EditGenre()
        {
            if (SelectedGenre != null)
            {
                var copy = new Genre
                {
                    Id = SelectedGenre.Id,
                    Name = SelectedGenre.Name,
                    Description = SelectedGenre.Description
                };
                
                var viewModel = new GenreEditViewModel(copy, _libraryService);
                var window = new GenreEditWindow(viewModel);
                if (window.ShowDialog() == true)
                {
                    LoadGenres();
                }
            }
        }
        
        private async Task DeleteGenre()
        {
            if (SelectedGenre != null)
            {
                if (MessageBox.Show("Вы ряльно хотите удалить жанр?", "Подтверждение",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _libraryService.DeleteGenreAsync(SelectedGenre.Id);
                        LoadGenres();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении жанра: {ex.Message}");
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