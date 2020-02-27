using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public abstract class PaginatedRequestBase
    {
        protected int _pageNumber;
        private int _pageSize;
        private int _maxPageSize;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 1 ? value : 1;
        }


        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? 
                               _maxPageSize 
                              : value < 0 ? _pageSize : value;
        }

        public PaginatedRequestBase()
        {
            var (pageSize, maxPageSize) = DefaultPagesSize(); // will be implmemnted by interitors
            InitPageSizes(pageSize, maxPageSize);
        }

        protected abstract (int defaultPageSize, int maxPageSize) DefaultPagesSize();

        private void InitPageSizes(int pageSize, int maxPageSize)
        {
            _pageSize = pageSize > 0 ? pageSize : 10;
            _maxPageSize = maxPageSize > 0 ? maxPageSize : 50;
        }
    }



}
