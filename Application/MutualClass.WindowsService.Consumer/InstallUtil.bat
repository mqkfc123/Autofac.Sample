
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe E:\MutualClass.WindowsService\MutualClass.WindowsService.Consumer.exe 
net start MutualClassConsumerServiceZxsj
sc config MutualClassConsumerServiceZxsj start= auto
pause