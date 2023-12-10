using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Cores.Interfaces;
using Post.GeneratedServices;
using System.Collections.ObjectModel;

namespace EasyMicroservices.UI.Posts.ViewModels.Articles
{
    public class FilterArticlesListViewModel : BaseViewModel
    {
        public FilterArticlesListViewModel(ArticleClient articleClient)
        {
            _articleClient = articleClient;
            SearchCommand = new TaskRelayCommand(this, Search);
            DeleteCommand = new TaskRelayCommand<ArticleContract>(this, Delete);
            SearchCommand.Execute(null);
        }

        public ICommandAsync SearchCommand { get; set; }
        public ICommandAsync DeleteCommand { get; set; }

        public Action<ArticleContract> OnDelete { get; set; }
        readonly ArticleClient _articleClient;
        ArticleContract _SelectedArticleContract;
        public ArticleContract SelectedArticleContract
        {
            get => _SelectedArticleContract;
            set
            {
                _SelectedArticleContract = value;
                OnPropertyChanged(nameof(SelectedArticleContract));
            }
        }

        public int Index { get; set; } = 0;
        public int Length { get; set; } = 10;
        public int TotalCount { get; set; }
        public string SortColumnNames { get; set; }
        public ObservableCollection<ArticleContract> Articles { get; set; } = new ObservableCollection<ArticleContract>();

        private async Task Search()
        {
            var filteredResult = await _articleClient.FilterAsync(new FilterRequestContract()
            {
                IsDeleted = false,
                Index = Index,
                Length = Length,
                SortColumnNames = SortColumnNames
            }).AsCheckedResult(x => (x.Result, x.TotalCount));

            Articles.Clear();
            TotalCount = (int)filteredResult.TotalCount;
            foreach (var article in filteredResult.Result)
            {
                Articles.Add(article);
            }
        }

        public async Task Delete(ArticleContract contract)
        {
            await _articleClient.SoftDeleteByIdAsync(new Int64SoftDeleteRequestContract()
            {
                Id = contract.Id,
                IsDelete = true
            }).AsCheckedResult(x => x);
            Articles.Remove(contract);
            OnDelete?.Invoke(contract);
        }

        public override Task OnError(Exception exception)
        {
            return base.OnError(exception);
        }

        public override Task DisplayFetchError(ServiceContracts.ErrorContract errorContract)
        {
            return base.DisplayFetchError(errorContract);
        }
    }
}

