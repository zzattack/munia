from django.urls import *
from .views import *

urlpatterns = [
	re_path(r'^$', index),
	re_path(r'^(?i)version_check$', version_check),
	re_path(r'^(?i)get_latest$', get_latest),
	re_path(r'^(?i)report_bug$', report_bug),
]