using DesafioB3.Test.Domain;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;

namespace DesafioB3.Test
{
    public class EmailReader
    {
        private ImapSettings _settings;
        public EmailReader(IOptions<ImapSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<(string Subject, string Body)?> WaitForEmailAsync(string subjectContains, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            var deadline = DateTime.UtcNow.Add(timeout);

            while (DateTime.UtcNow < deadline)
            {
                using var client = new ImapClient();
                await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl, cancellationToken);
                await client.AuthenticateAsync(_settings.Email, _settings.Password, cancellationToken);

                var inbox = client.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly, cancellationToken);

                var uids = await inbox.SearchAsync(SearchQuery.All, cancellationToken);

                for (int i = uids.Count - 1; i >=  0; i--)
                {
                    var message = await inbox.GetMessageAsync(uids[i], cancellationToken);

                    if (!string.IsNullOrWhiteSpace(message.Subject) && message.Subject.Contains(subjectContains, StringComparison.OrdinalIgnoreCase))
                    {
                        var body = message.TextBody ?? message.HtmlBody ?? string.Empty;
                        await client.DisconnectAsync(true, cancellationToken);
                        return (message.Subject, body);
                    }
                }
                await client.DisconnectAsync(true, cancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
            return null;
        }
    }
}
