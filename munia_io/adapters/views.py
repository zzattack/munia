from django.shortcuts import render
from django.http import HttpResponse


def index(request):
    return render(request, 'adapters/index.html', {})

def overview(request):
    return render(request, 'adapters/overview.html', {})

def faq(request):
    return render(request, 'adapters/faq.html', {})

def tech(request):
    return render(request, 'adapters/tech.html', {})

def sources(request):
    return render(request, 'adapters/sources.html', {})

def skins(request):
    return render(request, 'adapters/skins.html', {})

def fw_update(request):
    return render(request, 'adapters/fw_update.html', {})

def pricing(request):
    return render(request, 'adapters/pricing.html', {})

def contact(request):
    return render(request, 'adapters/contact.html', {})
