from django.conf.urls import url

from . import views

urlpatterns = [
    url(r'^(?i)(?:index.*)?$', views.index, name='index'),
    url(r'^(?i)(?:overview.*)?$', views.overview, name='overview'),
    url(r'^(?i)faq.*$', views.faq, name='faq'),

    url(r'^(?i)tech.*$', views.tech, name='tech'),
    url(r'^(?i)sources.*$', views.sources, name='sources'),
    url(r'^(?i)skins.*$', views.skins, name='skins'),
    url(r'^(?i)fw.*$', views.fw_update, name='fw_update'),

    url(r'^(?i)pricing(?:-_\.#)(eu|usa)$', views.pricing, name='pricing'),
    url(r'^(?i)contact.*$', views.contact, name='contact'),
]
