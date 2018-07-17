from django.conf.urls import url, include
from django.contrib import admin
from django.conf import settings
from django.conf.urls.static import static
import adapters.urls
import tool.urls
import home.urls

urlpatterns = [
	url(r'^(?:adapters|munia)/', include(adapters.urls)),
	url(r'^tool/', include(tool.urls)),
	url(r'^admin/', admin.site.urls),
	url(r'^', include(home.urls)),
] + static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
