using System;
using System.Web;

namespace CzytnikRSS
{
    public class MyModule1 : IHttpModule
    {
        /// <summary>
        /// Konieczne będzie skonfigurowanie tego modułu w pliku Web.config
        /// sieci i zarejestrowanie go za pomocą programu IIS, aby można było go używać. Aby uzyskać więcej informacji,
        /// zobacz następujący link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //miejsce na kod czyszczenia.
        }

        public void Init(HttpApplication context)
        {
            // Poniżej znajduje się przykład sposobu obsługi zdarzenia LogRequest i udostępnienia 
            // implementacji niestandardowego rejestrowania dla tego zdarzenia
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public void OnLogRequest(Object source, EventArgs e)
        {
            //tutaj można wprowadzić logikę niestandardowego rejestrowania
        }
    }
}
