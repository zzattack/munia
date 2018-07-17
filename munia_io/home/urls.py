from django.conf.urls import *
from home.views import *

urlpatterns = [
    url(r'^(?i)(?:index.*)?$', index, name='index'),
    url(r'^(?i)contact$', contact, name='contact'),
]