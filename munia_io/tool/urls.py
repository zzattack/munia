from django.conf.urls import *
from .views import *

urlpatterns = [
	url(r'^$', index),
	url(r'^version_check$', version_check),
	url(r'^get_latest$', get_latest),
	url(r'^report_bug$', report_bug),
]