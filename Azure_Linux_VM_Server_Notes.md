Server Requirements
===============================
1) Ubuntu 16.04+
2) DotNet Core SDK 3.1
	A) get MS repo : wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	B) Add package: dpkg -i packages-microsoft-prod.deb
	C) Install .netcore: 
		apt-get update; 
		apt-get install apt-transport-https 
		apt-get update 
		apt-get install dotnet-sdk-3.1
3) Postgress
	Reference: https://www.digitalocean.com/community/tutorials/how-to-install-and-use-postgresql-on-ubuntu-18-04
	A) Install packages: 
		apt update
		apt install postgresql postgresql-contrib
	B) Create password user/role:
		create db user: psql% CREATE ROLE <username> with PASSWORD '<password>' LOGIN;
		create system user: sudo adduser <username>
	C) Create DB
		sudo -u postgres createdb <username>
	D) Check new role and db
		sudo -u <username> psql
		psql% \conninfo
4) Nginx/LetsEncrypt https://mhagemann.medium.com/how-to-secure-nginx-with-certbot-on-ubuntu-16-10-4a66956cd49c
	A) Base Nginx: sudo apt update; sudo apt install nginx
		i) Setup server block /etc/nginx/sites-available/<servername.domain.com>
			server {  
				server_name <www.example.com> <example.com>;
				return 301 https://<example.com>$request_uri;
			}
			server {  
				listen 443 ssl http2;
				server_name www.example.com;
				include snippets/ssl-params.conf;
				return 301 https://example.com$request_uri;
			}
			server {  
				listen 443 ssl http2;
				server_name example.com;
				include snippets/ssl-params.conf;
				root /var/www/<example.com>;
				index index.html;
				location / {
					try_files $uri $uri/ =404;
				}
			}
		ii) Enable server block: ln -s /etc/nginx/sites-available/<example.com> /etc/nginx/sites-enabled/
	B) LetsEncrypt (Ref: https://www.digitalocean.com/community/tutorials/how-to-secure-nginx-with-let-s-encrypt-on-ubuntu-18-04)					
		i)   add apt repo: #sudo add-apt-repository ppa:certbot/certbot
		ii)  cerbot: #sudo apt install python-certbot-nginx
	C) Install cert: certbot certonly --nginx
5) Sitemanager Service	
	A) Install agent or copy code: /var/www/<servername>
	B) Keys/Config dir: mkdir /etc/sitemanager/ (installed by agent if using TFS Pipeline)
	C) Systemd service: vi /etc/systemd/system/sitemanager.service
		[Unit]
		Description=Sitemanager .NetCore Application
		[Service]
		WorkingDirectory=/var/www/test.socialslice.net
		ExecStart=/usr/bin/dotnet /var/www/<servername>/SiteManager.V4.WebUI.dll
		Restart=always
		# Restart service after 10 seconds if the dotnet service crashes:
		RestartSec=10
		KillSignal=SIGINT
		SyslogIdentifier=sitemanager-app
		User=www-data
		Environment=ASPNETCORE_ENVIRONMENT=Production
		Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
		# If you need to run multiple services on different ports set
		# the ports environment variable here:
		# Environment=ASPNETCORE_URLS=http://localhost:6000
		[Install]
		WantedBy=multi-user.target
	D) Enable/Start/Monitor
		systemctl enable sitemanager.service
		systemctl start sitemanager.service
		systemctl status sitemanager.service
		journalctl -fu sitemanager.service	
		netstat -an | grep LIST (port 5000/5001)

SSL Self Signed (Optional)
Create Key in /etc/sitemanager (identity jwt signing certificate) see appsettings:identity
==============================================
https://kimsereyblog.blogspot.com/2018/07/self-signed-certificate-for-identity.html

Create Key in /etc/sitemanager (internal kesteral 50001 cert (localhost)) see appsettings:sslcertificate
==============================================
https://kimsereyblog.blogspot.com/2018/07/self-signed-certificate-for-identity.html
	
Install cert to local ca-authrity
==============================================
https://askubuntu.com/questions/645818/how-to-install-certificates-for-command-line

NGINX SSL (Self Signed) (public facing nginx ssl)
================================================================================
https://www.digitalocean.com/community/tutorials/how-to-create-a-self-signed-ssl-certificate-for-nginx-in-ubuntu-16-04

create cert: 
sudo openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /etc/ssl/private/nginx-selfsigned.key -out /etc/ssl/certs/nginx-selfsigned.crt

1-create random/dh group
2-sudo openssl dhparam -out /etc/ssl/certs/dhparam.pem 2048
3-nginx snippets
>>>>/etc/nginx/snippets/self-signed.conf
	ssl_certificate /etc/ssl/certs/nginx-selfsigned.crt;
	ssl_certificate_key /etc/ssl/private/nginx-selfsigned.key;
>>>>/etc/nginx/snippets/ssl-params.conf
	# from https://cipherli.st/
	# and https://raymii.org/s/tutorials/Strong_SSL_Security_On_nginx.html

	ssl_protocols TLSv1 TLSv1.1 TLSv1.2;
	ssl_prefer_server_ciphers on;
	ssl_ciphers "EECDH+AESGCM:EDH+AESGCM:AES256+EECDH:AES256+EDH";
	ssl_ecdh_curve secp384r1;
	ssl_session_cache shared:SSL:10m;
	ssl_session_tickets off;
	ssl_stapling on;
	ssl_stapling_verify on;
	resolver 8.8.8.8 8.8.4.4 valid=300s;
	resolver_timeout 5s;
	# Disable preloading HSTS for now.  You can use the commented out header line that includes
	# the "preload" directive if you understand the implications.
	#add_header Strict-Transport-Security "max-age=63072000; includeSubdomains; preload";
	add_header Strict-Transport-Security "max-age=63072000; includeSubdomains";
	# note this causes weird in local testing...:add_header X-Frame-Options DENY;
	add_header X-Content-Type-Options nosniff;

	ssl_dhparam /etc/ssl/certs/dhparam.pem;

