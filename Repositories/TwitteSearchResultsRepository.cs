using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Models;
using Twitter.DAL;

namespace Twitter.Repositories
{
    public class TwitteSearchResultsRepository: IDataRepository
    {
        public List<Status> Get()
        {
            using (TwitterDbContext db = new TwitterDbContext())
            {
                var twitts = db.t_TwitterResults.ToList();
                return twitts;
            }
        }

        public void Add(List<Status> results)
        {
            using (TwitterDbContext db = new TwitterDbContext())
            {
                db.t_TwitterResults.AddRange(results);
                db.SaveChanges();
            }
        }
    }
}
