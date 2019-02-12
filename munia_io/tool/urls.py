from django.urls import *
from .views import *

urlpatterns = [
	re_path(r'^$', index),
	re_path(r'^version_check(?i)$', version_check),
	re_path(r'^get_latest(?i)$', get_latest),
	re_path(r'^report_bug(?i)$', report_bug),
]