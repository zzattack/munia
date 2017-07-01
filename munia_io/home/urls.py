from django.conf.urls import *
from views import *

urlpatterns = [
    url(r'^(?i)(?:index.*)?$', index, name='index'),
    url(r'^(?i)contact$', contact, name='contact'),
]