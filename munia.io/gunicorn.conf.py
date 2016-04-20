import os

user = "root"
workers = 2
bind = "10.31.45.10:8003"
pidfile = "/var/run/gunicorn-munia.pid"
backlog = 2048
logfile = "/var/log/gunicorn-munia.log"
loglevel = "debug"
