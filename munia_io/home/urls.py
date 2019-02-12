from django.urls import *
from home.views import *

urlpatterns = [
	re_path(r'^(?i)(?:index.*)?$', index, name='index'),
	re_path(r'^(?i)devices', devices, name='devices'),
	re_path(r'^(?i)software', software, name='software'),
	re_path(r'^(?i)faq', faq, name='faq'),
	re_path(r'^(?i)contact', contact, name='contact'),
	re_path(r'^(?i)success', success, name='success'),
]
