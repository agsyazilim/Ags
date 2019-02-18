using System;
using System.Collections.Generic;
using System.Linq;
using Ags.Data.Core.Pages;
using Ags.Data.Core.Repository;
using Ags.Data.Domain.News;
using Ags.Services.Events;

namespace Ags.Services.NewsPapers
{
    public class NewsPaperServices :INewsPaperServices
    {
        private readonly IRepository<EnewsPaper> _enewsRepository;
        private readonly IRepository<NewsPaperCategory> _newsPaperCategoryRepository;
        private readonly IEventPublisher _eventPublisher;

        public NewsPaperServices(IRepository<EnewsPaper> enewsRepository, IRepository<NewsPaperCategory> newsPaperCategoryRepository, IEventPublisher eventPublisher)
        {
            this._enewsRepository = enewsRepository;
            _newsPaperCategoryRepository = newsPaperCategoryRepository;
            _eventPublisher = eventPublisher;
        }

        public void DeleteNews(EnewsPaper newsItem)
        {
            if(newsItem==null)
                throw new ArgumentNullException(nameof(newsItem));

            _enewsRepository.Delete(newsItem);
           _eventPublisher.EntityDeleted(newsItem);
        }

        public EnewsPaper GetNewsById(int newsId)
        {
           return _enewsRepository.GetById(newsId);

        }

        public EnewsPaper GetNewsByIdAs(int newsId)
        {
            IQueryable<EnewsPaper> query = _enewsRepository.TableNoTracking;
            IQueryable<EnewsPaper> result = query.Where(x => x.Id == newsId);
            return result.FirstOrDefault();
        }

        public IList<EnewsPaper> GetNewsByIds(int[] newsIds)
        {
            throw new NotImplementedException();
        }

        public IPagedList<EnewsPaper> GetAllNews(int categoryId = 0, DateTime? createTo = null, int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            IQueryable<EnewsPaper> query = _enewsRepository.Table;
            if (categoryId > 0)
            query = query.Where(x => x.NewsPaperCategoryId == categoryId);

            if (createTo.HasValue)
                query = query.Where(x => createTo.Value <= (x.CreateDate));
            List<EnewsPaper> result = query.ToList();
            return new PagedList<EnewsPaper>(result,pageIndex,pageSize);
        }

        public void InsertNews(EnewsPaper news)
        {
            if(news==null)
                throw new ArgumentNullException(nameof(news));
            _enewsRepository.Insert(news);
            _eventPublisher.EntityInserted(news);
        }

        public void UpdateNews(EnewsPaper news)
        {
            if (news == null)
                throw new ArgumentNullException(nameof(news));
            _enewsRepository.Update(news);
            _eventPublisher.EntityUpdated(news);
        }

        public IList<NewsPaperCategory> GetAllCategories()
        {
            IQueryable<NewsPaperCategory> query = _newsPaperCategoryRepository.Table;
            return query.ToList();
        }

        public EnewsPaper GetNewsCategoryById(int newsId)
        {
            if(newsId==0)
                throw new ArgumentNullException(nameof(newsId));
            IQueryable<EnewsPaper> query = _enewsRepository.Table;
            EnewsPaper catergory = query.FirstOrDefault(x => x.NewsPaperCategoryId == newsId);
            return catergory;
        }

        public NewsPaperCategory GetCategoriById(int id)
        {
            return _newsPaperCategoryRepository.GetById(id);
        }

        public IPagedList<NewsPaperCategory> GetAllCategoriesNews(string name,int pageIndex = 0, int pageSize = Int32.MaxValue)
        {
            IQueryable<NewsPaperCategory> result = _newsPaperCategoryRepository.Table;
            if (name != null)
                result = result.Where(x => x.Name.Contains(name));
            List<NewsPaperCategory> categories = result.ToList();
            return  new PagedList<NewsPaperCategory>(categories,pageIndex,pageSize);
        }

        public void DeleteNewsCategory(NewsPaperCategory newsCategory)
        {
            if (newsCategory == null)
                throw new ArgumentNullException(nameof(newsCategory));
            _newsPaperCategoryRepository.Delete(newsCategory);
            _eventPublisher.EntityDeleted(newsCategory);
        }

        public void InsertCategory(NewsPaperCategory category)
        {
            if(category==null)
                throw new ArgumentNullException(nameof(category));
            _newsPaperCategoryRepository.Insert(category);
            _eventPublisher.EntityInserted(category);
        }

        public void UpdateCategory(NewsPaperCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            _newsPaperCategoryRepository.Update(category);
            _eventPublisher.EntityUpdated(category);

        }
    }
}