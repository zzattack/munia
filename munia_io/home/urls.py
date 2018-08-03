from django.conf.urls import *
from home.views import *

urlpatterns = [
	url(r'^(?i)(?:index.*)?$', index, name='index'),
	url(r'^(?i)devices', devices, name='devices'),
	url(r'^(?i)software', software, name='software'),
	url(r'^(?i)faq', faq, name='faq'),
	url(r'^(?i)contact$', contact, name='contact'),
	url(r'^(?i)success', success, name='success'),
]
