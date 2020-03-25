import { Book } from "../_models/Book";
import React from "react";
import "./BookCard.css";

export interface BookCardProps {
    book: Book;
    onClick: (book: Book) => void;
    buttonText?: string;
}

export function BookCard(props: BookCardProps) {
    return (
        <div className="BookCard">
            <h2>
                {props.book.title}
            </h2> 
            <p>
                {props.book.summary}
            </p>
            <button onClick={() => props.onClick(props.book)}>Loan</button>
        </div>
    )
}