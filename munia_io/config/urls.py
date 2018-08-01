from django.conf import settings
from django.conf.urls import url, include
from django.conf.urls.static import static
from django.contrib import admin
from django.urls import path

import adapters.urls
import home.urls
import tool.urls

urlpatterns = [
	url(r'^(?:adapters|munia)/', include(adapters.urls)),
	url(r'^tool/', include(tool.urls)),
	url(r'^', include(home.urls)),
	path(r'admin/', admin.site.urls),
] + static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
