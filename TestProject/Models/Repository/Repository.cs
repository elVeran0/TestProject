using System;

namespace TestProject.Models
{
    public class Repository
    {
        private Context context;

        public Repository(Context context = null)
        {
            if (context == null)
                this.context = new Context();
            else
                this.context = context;
        }

        public Context Context()
        { 
            return context;
        }
    }
    
}