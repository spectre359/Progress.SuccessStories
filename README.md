# Progress.SuccessStories
Sitefinity CMS project

My default SQL server instance name is SQLEXPRESS. I think it's worth mentioning, because the name Sitefinity was suggesting was SQLExpress, so you may have to change this setting to work with your SQL server.

The administrator credentials are: Username: philsw359@gmail.com Pass: sitefinity

You need to login in order to create new Success Stories. The pages are hidden for non admins. There are 2 predifined emails in the web.config file that will receive email notifications. All current clients will receive a toast notification. The toast notifications use SignalR with Toastr.

Elmah has been enabled as a logging solution. You can access it by entering /elmah.axd. (https://localhost:44348/elmah.axd)
