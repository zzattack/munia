from django.shortcuts import render
from django.http import HttpResponse


def index(request):
    return render(request, 'home/index.html', {})

def overview(request):
    return render(request, 'home/overview.html', {})

def faq(request):
    return render(request, 'home/faq.html', {})

def tech(request):
    return render(request, 'home/tech.html', {})

def sources(request):
    return render(request, 'home/sources.html', {})

def skins(request):
    return render(request, 'home/skins.html', {})

def fw_update(request):
    return render(request, 'home/fw_update.html', {})

def pricing(request):
    return render(request, 'home/pricing.html', {})

def contact(request):
    return render(request, 'home/contact.html', {})
