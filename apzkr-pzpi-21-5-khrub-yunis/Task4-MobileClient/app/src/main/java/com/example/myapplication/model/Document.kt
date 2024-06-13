package com.example.myapplication.model

data class Document(
    val documentID: Int,
    val username: String,
    val documentName: String,
    val documentType: String?,
    val documentPath: String?,
    val uploadDate: String
)
