from django.urls import *
from home.views import *

urlpatterns = [
	re_path(r'^(?:index.*)?$(?i)', index, name='index'),
	re_path(r'^devices.*$(?i)', devices, name='devices'),
	re_path(r'^software.*$(?i)', software, name='software'),
	re_path(r'^faq.*$(?i)', faq, name='faq'),
	re_path(r'^contact.*$(?i)', contact, name='contact'),
	re_path(r'^success.*$(?i)', success, name='success'),
]
