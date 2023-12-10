using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using Post.GeneratedServices;

namespace EasyMicroservices.UI.Posts.ViewModels.Articles
{
    public class AddOrUpdateArticleViewModel : BaseViewModel
    {
        public AddOrUpdateArticleViewModel(ArticleClient articleClient)
        {
            _articleClient = articleClient;
            SaveCommand = new TaskRelayCommand(this, Save);
            Clear();
        }

        public TaskRelayCommand SaveCommand { get; set; }

        readonly ArticleClient _articleClient;

        public Action OnSuccess { get; set; }
        ArticleContract _UpdateArticleContract;
        /// <summary>
        /// for update
        /// </summary>
        public ArticleContract UpdateArticleContract
        {
            get
            {
                return _UpdateArticleContract;
            }
            set
            {
                if (value is not null)
                {
                    Title = value.Title;
                    Summary = value.Summary;
                    Description = value.Description;
                    Body = value.Body;
                }
                _UpdateArticleContract = value;
            }
        }

        string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        string _Summary;
        public string Summary
        {
            get => _Summary;
            set
            {
                _Summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }


        string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }


        string _Body;
        public string Body
        {
            get => _Body;
            set
            {
                _Body = value;
                OnPropertyChanged(nameof(Body));
            }
        }

        public async Task Save()
        {
            if (UpdateArticleContract is not null)
                await UpdateArticle();
            else
                await AddArticle();
            OnSuccess?.Invoke();
        }

        public async Task AddArticle()
        {
            await _articleClient.AddAsync(new CreateArticleRequestContract()
            {
                Titles = GetLanguageData(Title),
                Summaries = GetLanguageData(Summary),
                Bodies = GetLanguageData(Body),
                Descriptions = GetLanguageData(Description)
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        List<LanguageDataContract> GetLanguageData(string text)
        {
            return new List<LanguageDataContract>()
            {
                new LanguageDataContract()
                {
                     Data = text,
                     Language = "fa-IR"
                }
            };
        }

        public override Task OnError(Exception exception)
        {
            return base.OnError(exception);
        }

        public override Task DisplayFetchError(ServiceContracts.ErrorContract errorContract)
        {
            return base.DisplayFetchError(errorContract);
        }

        public async Task UpdateArticle()
        {
            await _articleClient.UpdateChangedValuesOnlyAsync(new UpdateArticleRequestContract()
            {
                Id = UpdateArticleContract.Id,
                Titles = GetLanguageData(Title),
                Summaries = GetLanguageData(Summary),
                Bodies = GetLanguageData(Body),
                Descriptions = GetLanguageData(Description)
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        public void Clear()
        {
            Title = "";
            Summary = "";
            Body = "";
            Description = "";
        }
    }
}
