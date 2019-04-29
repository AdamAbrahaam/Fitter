﻿using System;
using System.Collections.Generic;
using Fitter.BL.Model;

namespace Fitter.BL.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        void Create(CommentModel comment);
        void Delete(Guid id);

        IList<CommentModel> GetCommentsForPost(Guid id);
        IList<Guid> SearchInComments(string substring, Guid id);
    }
}