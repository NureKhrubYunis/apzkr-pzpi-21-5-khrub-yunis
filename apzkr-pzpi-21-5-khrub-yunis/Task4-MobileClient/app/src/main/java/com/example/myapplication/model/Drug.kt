package com.example.myapplication.model

data class Drug(
    val drugID: Int,
    val drugName: String,
    val description: String,
    val manufacturer: String,
    val price: Double,
    val stockQuantity: Int,
    val releaseDate: String
)
